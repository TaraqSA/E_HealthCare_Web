
//get all neccessary variables selected
const track = document.querySelector('.carousel-slide');
const slides = Array.from(track.children);
//console.log(slides);

const nextButton = document.querySelector('.next-arrow');
const prevButton = document.querySelector('.prev-arrow');
const dotContainer = document.querySelector('.dot-container');
const dots = Array.from(dotContainer.children);

const slideWidth = slides[0].getBoundingClientRect().width;
//console.log(slideWidth);

//arrange images go to left
const setSlidePosition = (slides, index) => {
    slides.style.left = slideWidth * index + 'px';
};
slides.forEach(setSlidePosition);
//console.log(slides[0]);


//make both button work
//functions
const hideShowArrow = (slides, prevButton, nextButton, targetIndex) => {
    if (targetIndex === 0) {
        prevButton.classList.add('is-hidden');
        nextButton.classList.remove('is-hidden');
    } else if (targetIndex === slides.length - 1) {
        nextButton.classList.add('is-hidden');
        prevButton.classList.remove('is-hidden');
    } else {
        nextButton.classList.remove('is-hidden');
        prevButton.classList.remove('is-hidden');
    }
}


const moveToSlide = (track, currentSlide, targetSlide) => {
    track.style.transform = 'translateX(-' + targetSlide.style.left + ')';

    currentSlide.classList.remove('IsActive');
    targetSlide.classList.add('IsActive');
}

const updateDots = (currentDot, targetDot) => {
    currentDot.classList.remove('IsActive');
    targetDot.classList.add('IsActive');
}


//when click on next move next
nextButton.addEventListener('click', e => {
    const currentSlide = track.querySelector('.IsActive');
    const nextSlide = currentSlide.nextElementSibling;
    const currentDot = dotContainer.querySelector('.IsActive');
    const targetDot = currentDot.nextElementSibling;
    const nextIndex = slides.findIndex(i => i === nextSlide);
    moveToSlide(track, currentSlide, nextSlide);
    updateDots(currentDot, targetDot);

    hideShowArrow(slides, prevButton, nextButton, nextIndex);
});

//when click on prev arrow move prev
prevButton.addEventListener('click', e => {
    const currentSlide = track.querySelector('.IsActive');
    const prevSlide = currentSlide.previousElementSibling;
    const currentDot = dotContainer.querySelector('.IsActive');
    const targetDot = currentDot.previousElementSibling;
    const slideIndex = slides.findIndex(index => index === prevSlide);

    moveToSlide(track, currentSlide, prevSlide);
    updateDots(currentDot, targetDot);
    hideShowArrow(slides, prevButton, nextButton, slideIndex);


});

// change dots according to slides
const currentDot = dotContainer.querySelector('.IsActive');

dotContainer.addEventListener('click', e => {
    const targetDot = e.target.closest('button');
    if (!targetDot) return;
    const currentSlide = track.querySelector('.IsActive');
    const currentDot = dotContainer.querySelector('.IsActive');
    const targetDotIndex = dots.findIndex(d => d === targetDot);
    const targetSlide = slides[targetDotIndex];

    moveToSlide(track, currentSlide, targetSlide);
    updateDots(currentDot, targetDot);
    hideShowArrow(slides, prevButton, nextButton, targetDotIndex);

});






//disease carousel 

document.addEventListener('click', e => {
    let arrowButton;
    if (e.target.matches('.arrow-container')) {
        arrowButton = e.target;
    } else {
        arrowButton = e.target.closest('.arrow-container');
    }
    if (arrowButton != null) {
        onArrowButtonClick(arrowButton);
    }
})

function onArrowButtonClick(arrowButton) {
    const slider = arrowButton.closest('.main-slide').querySelector('.slide-container');
    const sliderIndex = parseInt(getComputedStyle(slider).getPropertyValue('--slider-index'));

    const slideCount = slider.childElementCount;

    if (arrowButton.classList.contains('prev-slide-arrow') && sliderIndex > 0) {
        console.log(sliderIndex - 1);
        slider.style.setProperty('--slider-index', sliderIndex - 1);


    } else if (arrowButton.classList.contains('next-slide-arrow') && sliderIndex <= slideCount / (4 * sliderIndex)) {
        console.log(sliderIndex+1)
        slider.style.setProperty('--slider-index', sliderIndex + 1);
        
    }
}

