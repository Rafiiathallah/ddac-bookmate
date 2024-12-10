// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener("DOMContentLoaded", function () {
  const overlay = document.querySelector(".overlay");
  const panel = document.querySelector(".book-details-panel");
  const closeButton = document.querySelector(".close-button");

  // Helper function for closing animation
  function animateAndClose() {
    if (panel) {
      panel.classList.add("closing");
      panel.classList.remove("active");

      // Wait for animation to complete before redirecting
      setTimeout(() => {
        window.location.href = "/Books";
      }, 100); // Match this with animation duration
    }
  }

  // Close button click handler
  if (closeButton) {
    closeButton.addEventListener("click", function (e) {
      e.preventDefault();
      animateAndClose();
    });
  }

  // Close panel when clicking overlay
  if (overlay) {
    overlay.addEventListener("click", function () {
      animateAndClose();
    });
  }

  // Close panel with Escape key
  document.addEventListener("keydown", function (e) {
    if (e.key === "Escape" && panel && panel.classList.contains("active")) {
      animateAndClose();
    }
  });
});
