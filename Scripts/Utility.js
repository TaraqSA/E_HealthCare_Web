
$(document).ready(function () {
    const accordion = document.getElementsByClassName('custom-accordion');

    for (i = 0; i < accordion.length; i++) {
        accordion[i].addEventListener('click', function () {
            this.classList.toggle('active')
        })
    }

});
