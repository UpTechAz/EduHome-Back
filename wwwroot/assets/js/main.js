window.addEventListener("DOMContentLoaded", function () {
  //scroll ile headerin olcusunun kicilmesi

  let header = document.querySelector("#header");

  function handleScroll() {
    let scrollPosition = window.scrollY;

    if (scrollPosition >= 100) {
      header.classList.add("sticky-header");
    } else {
      header.classList.remove("sticky-header");
    }
  }

  window.addEventListener("scroll", handleScroll);

  //header search input-un acilmasi
  let headerSearchBtn = document.querySelector(".header-search-button");
  let searchArea = document.querySelector(".search-input-div");

  headerSearchBtn.addEventListener("click", function (e) {
    e.preventDefault();
    searchArea.classList.toggle("search-input-open");
  });

  //header-in burger menu acilmasi

  let burgerBtn = document.querySelector(".burger-btn");
  let mobMenu = document.querySelector(".mobile-header");

  burgerBtn.addEventListener("click", function (e) {
    e.preventDefault();
    mobMenu.classList.toggle("show-mobile-header");

    let icon = burgerBtn.querySelector("i");
    if (icon.classList.contains("fa-bars")) {
      icon.classList.remove("fa-bars");
      icon.classList.add("fa-xmark");
    } else {
      icon.classList.remove("fa-xmark");
      icon.classList.add("fa-bars");
    }
  });

  //mob-menu-da toggle-menularin acilmasi

  let plusMinusBtns = document.querySelectorAll(".plus-minus-btn");

  for (let btn of plusMinusBtns) {
    btn.addEventListener("click", function (e) {
      e.preventDefault();
      let index = btn.getAttribute("data-index");
      let toggleMenu = document.querySelector(
        `.mob-nav-toggle[data-index="${index}"]`
      );

      toggleMenu.classList.toggle("show-toggle");

      if (toggleMenu.classList.contains("show-toggle")) {
        btn.textContent = "-";
      } else {
        btn.textContent = "+";
      }
    });
  }

  //hero-slide

  $(".slider-owl").owlCarousel({
    loop: true,
    nav: true,
    animateOut: "fadeOut",
    animateIn: "fadeIn",
    dots: false,
    smartSpeed: 2500,
    responsive: {
      0: {
        items: 1,
      },
      768: {
        items: 1,
      },
      1000: {
        items: 1,
      },
    },
  });

  $(".owl-carousel").on("changed.owl.carousel", function (event) {
    var currentIndex = event.item.index;
    $(".owl-item").removeClass("active");
    $(".owl-item").eq(currentIndex).addClass("active");
  });

  // testimonial and about slide

  $(".testimonial-owl").owlCarousel({
    loop: true,
    nav: true,
    dots: false,
    nav: false,
    navText: [
      '<i class="fa fa-angle-left"></i>',
      '<i class="fa fa-angle-right"></i>',
    ],
    responsive: {
      0: {
        items: 1,
      },
      768: {
        items: 1,
      },
      1000: {
        items: 1,
      },
    },
  });

  //home ve about hissesindeki video-nun acilib baglanmasi

  let popup_btn = document.querySelector(".video-popup-btn");
  let close_popup_btn = document.querySelector(".popup-close-btn");
  let popup_video_backgruond = document.querySelector(
    ".popup-video-background"
  );
  let popup_video_main_holder = document.querySelector(
    ".popup-video-main-holder"
  );
  let iframeSrc = document.querySelector(".iframe-holder iframe").src;

  popup_btn.addEventListener("click", function (e) {
    e.preventDefault();
    document.body.style.overflow = "hidden";
    popup_video_backgruond.classList.add("popup-show");
    popup_video_main_holder.classList.add("popup-show");
    document.querySelector(".iframe-holder iframe").src = iframeSrc;
  });

  close_popup_btn.addEventListener("click", function () {
    document.body.style.overflow = "";
    popup_video_backgruond.classList.remove("popup-show");
    popup_video_main_holder.classList.remove("popup-show");
    document.querySelector(".iframe-holder iframe").src = "";
  });
});

// courses hissesindeki search inputunda inputun temizlenmesi
// ve inputun icinde x duymesinin gorunub ortadan qaldirilmasi

let courseSearchInput = document.querySelector(
  ".courses-area .container .row .col-12 .course-form .input-div .course-search-input"
);
let courseClearBtn = document.querySelector(
  ".courses-area .container .row .col-12 .course-form .input-div .course-clear-btn"
);

courseSearchInput.addEventListener("input", function () {
  courseClearBtn.classList.add("clear-btn-yes");
});

courseClearBtn.addEventListener("click", function (e) {
  e.preventDefault();
  courseSearchInput.value = "";
  courseClearBtn.classList.remove("clear-btn-yes");
});
