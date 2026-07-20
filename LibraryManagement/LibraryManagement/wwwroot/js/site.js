document.addEventListener('DOMContentLoaded', function () {
    const sidebar = document.getElementById('sidebar');
    const overlay = document.getElementById('sidebarOverlay');
    const hamburger = document.getElementById('hamburgerBtn');

    // --- Active link highlighting ---
    const currentController = document.querySelector('.sidebar-link.active')?.dataset.controller
        || window.location.pathname.split('/')[1]?.toLowerCase();

    document.querySelectorAll('.sidebar-link').forEach(function (link) {
        const ctrl = link.dataset.controller;
        if (ctrl && ctrl.toLowerCase() === currentController) {
            link.classList.add('active');
        }
    });

    // --- Sidebar toggle (mobile) ---
    function openSidebar() {
        sidebar.classList.add('open');
        overlay.classList.add('active');
        document.body.style.overflow = 'hidden';
    }

    function closeSidebar() {
        sidebar.classList.remove('open');
        overlay.classList.remove('active');
        document.body.style.overflow = '';
    }

    hamburger.addEventListener('click', function () {
        if (sidebar.classList.contains('open')) {
            closeSidebar();
        } else {
            openSidebar();
        }
    });

    overlay.addEventListener('click', closeSidebar);

    // Close sidebar on Escape key
    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape' && sidebar.classList.contains('open')) {
            closeSidebar();
        }
    });

    // Close sidebar when a link is clicked (mobile)
    sidebar.querySelectorAll('.sidebar-link').forEach(function (link) {
        link.addEventListener('click', function () {
            if (window.innerWidth < 992) {
                closeSidebar();
            }
        });
    });
});
