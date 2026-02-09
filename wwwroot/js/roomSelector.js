

console.log("✅ roomSelector.js loaded");

                                            // UNUSED


const roomSelect = document.getElementById("room");

// roomCnt
const roomTitle = document.getElementById("roomTitle"); // <h3>
const roomDesc = document.getElementById("roomDesc");   // <p>
const roomImg = document.getElementById("roomImg");     // <img>

// Room data
const rooms = {
    101: {
        name: "ROOM 101",
        description: "With capacity of etc etc",
        image: "/images/Screenshot (14).png"
    },
    102: {
        name: "ROOM 102",
        description: "Spacious double room with balcony and garden view.",
        image: "/images/Screenshot (4).png"
    },
    
};

// change handler
roomSelect.addEventListener("change", function () {
    const value = this.value;
    const room = rooms[value];

    if (room) {
        //update heading
        roomTitle.textContent = room.name;

        // update desc
        roomDesc.textContent = room.description;

        // ipdate image
        roomImg.src = room.image;
        roomImg.alt = room.name;
    }
});





document.addEventListener("DOMContentLoaded", () => {
    const schedulePopup = document.getElementById("schedulePopup");
    const scheduleBtn = document.getElementById("scheduleBtn");
    const closePopup = document.getElementById("closePopup");

    // Open popup when clicking "Schedule"
    scheduleBtn.addEventListener("click", (e) => {
        e.stopPropagation();
        schedulePopup.style.display = "block";
    });

    // Close popup with Close button
    closePopup.addEventListener("click", () => {
        schedulePopup.style.display = "none";
    });

    // Close popup when clicking outside
    window.addEventListener("click", (e) => {
        if (schedulePopup.style.display === "block" && !schedulePopup.contains(e.target) && e.target !== scheduleBtn) {
            schedulePopup.style.display = "none";
        }
    });
});



// FOR POP UP FUNCT




const schedulePopup = document.getElementById("schedulePopup");
const popupHeader = document.getElementById("popupHeader"); // must exist in your HTML

let isDragging = false;
let offsetX = 0;
let offsetY = 0;

popupHeader.addEventListener("mousedown", (e) => {
    isDragging = true;
    offsetX = e.clientX - schedulePopup.offsetLeft;
    offsetY = e.clientY - schedulePopup.offsetTop;
    e.preventDefault(); // prevents text highlighting
});

document.addEventListener("mouseup", () => {
    isDragging = false;
});

document.addEventListener("mousemove", (e) => {
    if (isDragging) {
        schedulePopup.style.left = (e.clientX - offsetX) + "px";
        schedulePopup.style.top = (e.clientY - offsetY) + "px";
    }
});
