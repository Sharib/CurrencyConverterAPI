// API Configuration
const API_BASE = '/api/MaintenanceIncident';

// State Management
let incidents = [];
let currentIncident = null;

// Initialize App
document.addEventListener('DOMContentLoaded', () => {
    initializeTheme();
    initializeNavigation();
    initializeEventListeners();
    loadIncidents();
});

// Theme Management
function initializeTheme() {
    const savedTheme = localStorage.getItem('theme') || 'light';
    document.documentElement.setAttribute('data-theme', savedTheme);
    
    document.getElementById('themeToggle').addEventListener('click', () => {
        const currentTheme = document.documentElement.getAttribute('data-theme');
        const newTheme = currentTheme === 'light' ? 'dark' : 'light';
        document.documentElement.setAttribute('data-theme', newTheme);
        localStorage.setItem('theme', newTheme);
    });
}

// Navigation
function initializeNavigation() {
    const navItems = document.querySelectorAll('.nav-item');
    navItems.forEach(item => {
        item.addEventListener('click', (e) => {
            e.preventDefault();
            const view = item.getAttribute('data-view');
            switchView(view);
            
            // Update active state
            navItems.forEach(nav => nav.classList.remove('active'));
            item.classList.add('active');
        });
    });
}

function switchView(viewName) {
    const views = document.querySelectorAll('.view');
    views.forEach(view => view.classList.remove('active'));
    document.getElementById(`${viewName}-view`).classList.add('active');
    
    // Load data for specific views
    if (viewName === 'review') {
        loadReviewQueue();
    }
}

// Event Listeners
function initializeEventListeners() {
    // Create incident form
    document.getElementById('create-incident-form').addEventListener('submit', handleCreateIncident);
    
    // Filter
    document.getElementById('filter-status').addEventListener('change', handleFilterChange);
    
    // Modal
    document.querySelectorAll('.modal-close').forEach(btn => {
        btn.addEventListener('click', closeModal);
    });
    
    document.getElementById('approve-btn').addEventListener('click', () => handleApproval(true));
    document.getElementById('reject-btn').addEventListener('click', () => handleApproval(false));
    
    // Click outside modal to close
    document.getElementById('incident-modal').addEventListener('click', (e) => {
        if (e.target.id === 'incident-modal') {
            closeModal();
        }
    });
}

// API Functions
async function loadIncidents() {
    try {
        const response = await fetch(API_BASE);
        if (!response.ok) throw new Error('Failed to load incidents');
        const data = await response.json();
        incidents = data.incidents || [];
        updateDashboard();
        renderAllIncidents();
        renderRecentIncidents();
    } catch (error) {
        console.error('Error loading incidents:', error);
        showNotification('Failed to load incidents', 'error');
    }
}

async function handleCreateIncident(e) {
    e.preventDefault();
    
    const formData = new FormData(e.target);
    const data = {
        description: formData.get('description'),
        type: parseInt(formData.get('type')),
        serviceCentre: formData.get('serviceCentre'),
        cost: parseFloat(formData.get('cost'))
    };
    
    try {
        const response = await fetch(API_BASE, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });
        
        if (!response.ok) {
            const error = await response.json();
            throw new Error(error.title || 'Failed to create incident');
        }
        
        const result = await response.json();
        showNotification('Incident created successfully!', 'success');
        e.target.reset();
        await loadIncidents();
        switchView('incidents');
    } catch (error) {
        console.error('Error creating incident:', error);
        showNotification(error.message || 'Failed to create incident', 'error');
    }
}

async function handleFilterChange(e) {
    const status = e.target.value;
    
    if (!status) {
        renderAllIncidents();
        return;
    }
    
    try {
        const response = await fetch(`${API_BASE}?status=${status}`);
        if (!response.ok) throw new Error('Failed to filter incidents');
        const data = await response.json();
        const filtered = data.incidents || [];
        renderFilteredIncidents(filtered);
    } catch (error) {
        console.error('Error filtering incidents:', error);
        showNotification('Failed to filter incidents', 'error');
    }
}

async function handleApproval(approve) {
    if (!currentIncident) return;
    
    const reviewerName = prompt(approve ? 'Enter your name as reviewer:' : 'Enter your name:');
    if (!reviewerName) return;
    
    const reviewNotes = prompt(approve ? 'Approval notes (optional):' : 'Rejection reason:');
    
    try {
        const response = await fetch(`${API_BASE}/approve`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                incidentId: currentIncident.id,
                reviewedBy: reviewerName,
                reviewNotes: reviewNotes || (approve ? 'Approved' : 'Rejected')
            })
        });
        
        if (!response.ok) throw new Error('Failed to update incident');
        
        showNotification(approve ? 'Incident approved!' : 'Incident rejected!', 'success');
        closeModal();
        await loadIncidents();
        loadReviewQueue();
    } catch (error) {
        console.error('Error updating incident:', error);
        showNotification('Failed to update incident', 'error');
    }
}

// UI Rendering Functions
function updateDashboard() {
    const stats = {
        total: incidents.length,
        draft: incidents.filter(i => i.status === 0).length,
        pending: incidents.filter(i => i.status === 2).length,
        approved: incidents.filter(i => i.status === 3).length
    };
    
    document.getElementById('stat-total').textContent = stats.total;
    document.getElementById('stat-draft').textContent = stats.draft;
    document.getElementById('stat-pending').textContent = stats.pending;
    document.getElementById('stat-approved').textContent = stats.approved;
}

function renderRecentIncidents() {
    const container = document.getElementById('recent-incidents');
    const recent = incidents.slice(0, 5);
    
    if (recent.length === 0) {
        container.innerHTML = '<div class="empty-state">No incidents yet</div>';
        return;
    }
    
    container.innerHTML = recent.map(incident => createIncidentCard(incident)).join('');
    attachIncidentCardListeners();
}

function renderAllIncidents() {
    const container = document.getElementById('all-incidents');
    
    if (incidents.length === 0) {
        container.innerHTML = '<div class="empty-state">No incidents found</div>';
        return;
    }
    
    container.innerHTML = incidents.map(incident => createIncidentCard(incident)).join('');
    attachIncidentCardListeners();
}

function renderFilteredIncidents(filtered) {
    const container = document.getElementById('all-incidents');
    
    if (filtered.length === 0) {
        container.innerHTML = '<div class="empty-state">No incidents match the filter</div>';
        return;
    }
    
    container.innerHTML = filtered.map(incident => createIncidentCard(incident)).join('');
    attachIncidentCardListeners();
}

function loadReviewQueue() {
    const container = document.getElementById('review-incidents');
    const reviewQueue = incidents.filter(i => i.status === 0 || i.status === 1 || i.status === 2);
    
    if (reviewQueue.length === 0) {
        container.innerHTML = '<div class="empty-state">No incidents in review queue</div>';
        return;
    }
    
    container.innerHTML = reviewQueue.map(incident => createIncidentCard(incident)).join('');
    attachIncidentCardListeners();
}

function createIncidentCard(incident) {
    const statusText = getStatusText(incident.status);
    const typeText = getTypeText(incident.type);
    const date = new Date(incident.createdDate).toLocaleDateString();
    
    return `
        <div class="incident-card" data-id="${incident.id}">
            <div class="incident-header">
                <div class="incident-title">${escapeHtml(incident.description.substring(0, 100))}${incident.description.length > 100 ? '...' : ''}</div>
                <span class="status-badge status-${statusText.toLowerCase()}">${statusText}</span>
            </div>
            <div class="incident-meta">
                <span class="type-badge">${typeText}</span>
                <span>📍 ${escapeHtml(incident.serviceCentre)}</span>
                <span>💰 $${incident.cost.toFixed(2)}</span>
                <span>📅 ${date}</span>
            </div>
        </div>
    `;
}

function attachIncidentCardListeners() {
    document.querySelectorAll('.incident-card').forEach(card => {
        card.addEventListener('click', () => {
            const id = card.getAttribute('data-id');
            const incident = incidents.find(i => i.id === id);
            if (incident) showIncidentDetails(incident);
        });
    });
}

function showIncidentDetails(incident) {
    currentIncident = incident;
    const modal = document.getElementById('incident-modal');
    const detailsContainer = document.getElementById('incident-details');
    
    const statusText = getStatusText(incident.status);
    const typeText = getTypeText(incident.type);
    
    detailsContainer.innerHTML = `
        <div class="detail-row">
            <div class="detail-label">Incident ID</div>
            <div class="detail-value">${incident.id}</div>
        </div>
        <div class="detail-row">
            <div class="detail-label">Description</div>
            <div class="detail-value">${escapeHtml(incident.description)}</div>
        </div>
        <div class="detail-row">
            <div class="detail-label">Type</div>
            <div class="detail-value"><span class="type-badge">${typeText}</span></div>
        </div>
        <div class="detail-row">
            <div class="detail-label">Service Centre</div>
            <div class="detail-value">${escapeHtml(incident.serviceCentre)}</div>
        </div>
        <div class="detail-row">
            <div class="detail-label">Cost</div>
            <div class="detail-value">$${incident.cost.toFixed(2)}</div>
        </div>
        <div class="detail-row">
            <div class="detail-label">Status</div>
            <div class="detail-value"><span class="status-badge status-${statusText.toLowerCase()}">${statusText}</span></div>
        </div>
        <div class="detail-row">
            <div class="detail-label">Created Date</div>
            <div class="detail-value">${new Date(incident.createdDate).toLocaleString()}</div>
        </div>
        ${incident.reviewedBy ? `
            <div class="detail-row">
                <div class="detail-label">Reviewed By</div>
                <div class="detail-value">${escapeHtml(incident.reviewedBy)}</div>
            </div>
            <div class="detail-row">
                <div class="detail-label">Review Date</div>
                <div class="detail-value">${new Date(incident.reviewedDate).toLocaleString()}</div>
            </div>
            <div class="detail-row">
                <div class="detail-label">Review Notes</div>
                <div class="detail-value">${escapeHtml(incident.reviewNotes || 'N/A')}</div>
            </div>
        ` : ''}
    `;
    
    // Show/hide approve/reject buttons
    const approveBtn = document.getElementById('approve-btn');
    const rejectBtn = document.getElementById('reject-btn');
    if (incident.status === 3 || incident.status === 4) {
        approveBtn.style.display = 'none';
        rejectBtn.style.display = 'none';
    } else {
        approveBtn.style.display = 'inline-block';
        rejectBtn.style.display = 'inline-block';
    }
    
    modal.classList.add('active');
}

function closeModal() {
    document.getElementById('incident-modal').classList.remove('active');
    currentIncident = null;
}

// Helper Functions
function getStatusText(status) {
    const statuses = ['Draft', 'Submitted', 'Review', 'Approved', 'Rejected'];
    return statuses[status] || 'Unknown';
}

function getTypeText(type) {
    const types = ['Repair', 'Warranty Claim', 'Part Failure'];
    return types[type] || 'Unknown';
}

function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

function showNotification(message, type = 'info') {
    // Simple alert for now - could be enhanced with toast notifications
    if (type === 'error') {
        alert('Error: ' + message);
    } else {
        alert(message);
    }
}

// Sample data for initial testing
function loadSampleData() {
    const sampleIncidents = [
        {
            id: crypto.randomUUID(),
            description: "Engine failure on truck #124 - requires immediate attention",
            type: 0,
            serviceCentre: "Sydney AFG Service Centre",
            cost: 5500.00,
            status: 0,
            createdDate: new Date().toISOString()
        },
        {
            id: crypto.randomUUID(),
            description: "Warranty claim for transmission system",
            type: 1,
            serviceCentre: "Melbourne AFG Service Centre",
            cost: 8500.00,
            status: 2,
            createdDate: new Date(Date.now() - 86400000).toISOString()
        },
        {
            id: crypto.randomUUID(),
            description: "Brake system part failure - safety critical",
            type: 2,
            serviceCentre: "Brisbane AFG Service Centre",
            cost: 3200.00,
            status: 3,
            createdDate: new Date(Date.now() - 172800000).toISOString(),
            reviewedBy: "John Manager",
            reviewedDate: new Date(Date.now() - 86400000).toISOString(),
            reviewNotes: "Approved for immediate reimbursement"
        }
    ];
    
    // Store in localStorage as fallback
    localStorage.setItem('incidents', JSON.stringify(sampleIncidents));
}
