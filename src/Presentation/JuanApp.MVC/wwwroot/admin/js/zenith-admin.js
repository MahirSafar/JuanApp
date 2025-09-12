$(document).ready(function () {
    $(function () {
        var $container = $('.zenith-admin-container');
        var $hamburger = $('#hamburger');
        function updateHamburger() {
            if ($container.hasClass('sidebar-collapsed')) {
                $hamburger.removeClass('active');
            } else {
                $hamburger.addClass('active');
            }
        }
        $hamburger.on('click', function () {
            $container.toggleClass('sidebar-collapsed');
            updateHamburger();
        });
        updateHamburger();
        var currentPath = window.location.pathname.toLowerCase();
        $('.zenith-nav-item').each(function () {
            if (currentPath === $(this).attr('href').toLowerCase()) {
                $(this).addClass('active');
            }
        });
    });
});