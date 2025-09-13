function togglePassword(inputId) {
    const input = document.getElementById(inputId);
    const btn = input.nextElementSibling;
    if (input.type === 'password') {
        input.type = 'text';
        btn.innerHTML = '🙈';
    } else {
        input.type = 'password';
        btn.innerHTML = '👁️';
    }
}

function checkPasswordStrength(password) {
    const requirements = {
        length: password.length >= 8,
        uppercase: /[A-Z]/.test(password),
        lowercase: /[a-z]/.test(password),
        number: /\d/.test(password),
        special: /[!@#$%^&*(),.?":{ }|<>~`]/.test(password)
    };
    Object.keys(requirements).forEach(req => {
        const element = document.getElementById(`req-${req}`);
        if (requirements[req]) {
            element.classList.add('met');
        } else {
            element.classList.remove('met');
        }
    });
    const metRequirements = Object.values(requirements).filter(Boolean).length;
    const strengthBar = document.querySelector('.password-strength');
    const strengthText = document.getElementById('strengthText');
    strengthBar.className = 'password-strength';
    if (metRequirements < 3) {
        strengthBar.classList.add('strength-weak');
        strengthText.textContent = 'Weak password';
    } else if (metRequirements < 5) {
        strengthBar.classList.add('strength-medium');
        strengthText.textContent = 'Medium strength';
    } else {
        strengthBar.classList.add('strength-strong');
        strengthText.textContent = 'Strong password';
    }
}

document.getElementById('newPassword').addEventListener('input', (e) => {
    checkPasswordStrength(e.target.value);
});
