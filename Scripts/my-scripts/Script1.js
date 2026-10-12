document.addEventListener("DOMContentLoaded", function () {
    const container = document.getElementById('container');
    const registerBtn = document.querySelector('.register-btn');
    const loginBtn = document.querySelector('.login-btn');

    if (!container || !registerBtn || !loginBtn) return;

    registerBtn.addEventListener('click', () => {
        container.classList.remove('no-transition'); // bật transition
        container.classList.add('active');
    });

    loginBtn.addEventListener('click', () => {
        container.classList.remove('no-transition'); // bật transition
        container.classList.remove('active');
    });
});