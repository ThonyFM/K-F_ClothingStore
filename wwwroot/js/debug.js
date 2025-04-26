document.addEventListener('DOMContentLoaded', function() {
    const forms = document.querySelectorAll('form');

    forms.forEach(form => {
        form.addEventListener('submit', (e) => {
            console.log(`✅ Formulario enviado a: ${form.action}`);
            console.log(`✅ Método: ${form.method}`);
        });
    });

    console.log('🚀 Debug.js cargado correctamente.');
});
