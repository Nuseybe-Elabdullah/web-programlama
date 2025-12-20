// Site-wide JavaScript for Gym Management System

$(document).ready(function() {
    // Auto-hide alerts after 5 seconds
    setTimeout(function() {
        $('.alert').fadeOut('slow');
    }, 5000);

    // Confirm delete actions
    $('.delete-confirm').on('click', function(e) {
        if (!confirm('Are you sure you want to delete this item?')) {
            e.preventDefault();
        }
    });

    // Add loading spinner to forms (only if form is valid)
    $('form').on('submit', function(e) {
        var form = $(this);
        
        // Check if form has jQuery validation and if it's valid
        if (form.valid && typeof form.valid === 'function') {
            if (!form.valid()) {
                return; // Don't show loading if form is invalid
            }
        }
        
        var btn = form.find('button[type="submit"]');
        var originalHtml = btn.html();
        
        // Store original button text
        btn.data('original-html', originalHtml);
        btn.prop('disabled', true);
        btn.html('<span class="spinner-border spinner-border-sm me-2"></span>Yükleniyor...');
    });
});

// Utility function to format currency
function formatCurrency(amount) {
    return '$' + parseFloat(amount).toFixed(2);
}

// Utility function to format time
function formatTime(timeString) {
    var parts = timeString.split(':');
    var hours = parseInt(parts[0]);
    var minutes = parts[1];
    var ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12;
    return hours + ':' + minutes + ' ' + ampm;
}

// Show loading spinner
function showLoading(elementId) {
    $('#' + elementId).html('<div class="spinner-border text-primary" role="status"><span class="visually-hidden">Loading...</span></div>');
}

// Hide loading spinner
function hideLoading(elementId) {
    $('#' + elementId).html('');
}
