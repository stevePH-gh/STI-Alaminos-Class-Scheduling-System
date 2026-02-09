


// para di makalimot


/*TO DO
    [ONGOING] Room Profiles (seats, equips, etc)
    [ONGOING] Design
    [FINISHED] Fix Conflicted Sched
    [ONGOING] Accounts and Roles
    [NOT YET] Notifications
    [NOT YET] Reports Generation (Excel or txt file)
    [FINISHED] Subjects button (name, sched, time date, sub)
    [FINISHED] Drag and Drop

*/


const roomSelect = document.getElementById("room");

// roomCnt elements
const roomTitle = document.getElementById("roomTitle"); // <h3>
const roomDesc = document.getElementById("roomDesc");   // <p>
const roomImg = document.getElementById("roomImg");     // <img>
const scheduleList = document.getElementById("scheduleList"); // schedule popup

// Room data
const rooms = {
    /* 
       ROOM CODES
       101 = Comlab
       102 = F&B
       103 = KitchLab
       104 = BarLab
       105 = 201
       106 = 301 - Collapsable
       107 = 303
       108 = 401
       109 = 402
    */
    101: {
        name: "Computer Lab",
        description: "A dedicated space for hands-on technical training, workshops, and courses requiring computer access.  Ideal for large groups needing individual workspace.",
        image: "/images/rooms/rm_comlab.jpg"
    },
    102: {
        name: "F&B Lab",
        description: "A multi purpose training and demonstration room designed for hospitality, service, and beverage-related instruction.  Features flexible seating for discussions and presentations.",
        image: "/images/rooms/rm_fnb.jpg"
    },
    103: {
        name: "Kitchen lab",
        description: "A professional-grade instructional kitchen equipped for culinary classes, food preparation, and cooking demonstrations.\nPerfect for practical application and small group work.",
        image: "/images/rooms/rm_temp.png"
    },
    104: {
        name: "Bar lab",
        description: "Description here.",
        image: "/images/rooms/rm_temp.png"
    },
    105: {
        name: "Room 201",
        description: "(Capacity: 40) Located at 2nd floor left-wing, used for class lectures.",
        image: "/images/rooms/rm_201.png"
    },
    106: {
        name: "Room 301 - Collapsable",
        description: "A highly versatile room designed to be quickly reconfigured.  Cam be used as one large space for events, sound-separated meeting rooms.\nWith Emergency Exit",
        image: "/images/rooms/rm_301-collapsable.png"
    },
    107: {
        name: "Room 303",
        description: "(Capacity: 40) Located at 3rd floor left-wing, used for class lectures.\nWith Emergency Exit",
        image: "/images/rooms/rm_303.jpg"
    },
    108: {
        name: "Room 401",
        description: "(Capacity: 40) Located at top-most floor, used for class lectures.",
        image: "/images/rooms/rm_401.jpg"
    },
    109: {
        name: "Room 402",
        description: "(Capacity: 40) Located at top-most floor, used for class lectures",
        image: "/images/rooms/rm_402.jpg"
    }
};

// schedules storage per room
const schedules = {
    101: [],
    102: [],
    103: [],
    104: [],
    105: [],
    106: [],
    107: [],
    108: [],
    109: []
};

// change room info
roomSelect.addEventListener("change", function () {
    const value = this.value;
    const room = rooms[value];

    if (room) {
        roomTitle.textContent = room.name;
        roomDesc.textContent = room.description;
        roomImg.src = room.image;
        roomImg.alt = room.name;
    }

    renderSchedules(value); // show only schedules for that room
});
function ensureInitialRoomSelected() {
    let current = roomSelect.value;
    if (!current || current === "") {
        // MUST PiCK ROOMFiRST
        const opt = Array.from(roomSelect.options).find(o => o.value && o.value.trim() !== "");
        if (opt) {
            roomSelect.value = opt.value;
            current = opt.value;
        }
    }
    return current;
}

                                // MAIN FUNCTIONN

document.addEventListener("DOMContentLoaded", () => {
    const schedulePopup = document.getElementById("schedulePopup");
    const scheduleBtn = document.getElementById("scheduleBtn");
    const closePopup = document.getElementById("closePopup");
    const dragSchedule = document.getElementById("dragSchedule");
    const roomCnt = document.getElementById("roomCnt");

    // Restrict date onwards
    const today = new Date().toISOString().split("T")[0];
    scheduleDate.setAttribute("min", today);

    // popup open/close
    scheduleBtn.addEventListener("click", (e) => {
        e.stopPropagation();
        schedulePopup.classList.add("show");
    });

    closePopup.addEventListener("click", () => {
        schedulePopup.classList.remove("show");
    });

    window.addEventListener("click", (e) => {
        if (schedulePopup.classList.contains("show") &&
            !schedulePopup.contains(e.target) &&
            e.target !== scheduleBtn) {
            schedulePopup.classList.remove("show");
        }
    });

                                // schedule list hover mouse
    roomImg.addEventListener("mouseenter", () => {
        const currentRoom = roomSelect.value;
        if (schedules[currentRoom].length > 0) {
            scheduleList.style.display = "block";
        }
    });

    roomImg.addEventListener("mousemove", (e) => {
        if (scheduleList.style.display === "block") {
            scheduleList.style.left = (e.pageX + 15) + "px";
            scheduleList.style.top = (e.pageY + 15) + "px";
        }
    });

    roomImg.addEventListener("mouseleave", () => {
        scheduleList.style.display = "none";
    });

    // drag feature
    dragSchedule.addEventListener("dragstart", (e) => {
        const scheduleData = {
            instructor: document.getElementById("instructorName").value,
            subject: document.getElementById("subjectName").value,
            start: document.getElementById("startTime").value,
            end: document.getElementById("endTime").value,
            date: document.getElementById("scheduleDate").value || new Date().toISOString().split("T")[0]
        };
        e.dataTransfer.setData("application/json", JSON.stringify(scheduleData));
    });

    // click feature
    dragSchedule.addEventListener("click", () => {
        const data = {
            instructor: document.getElementById("instructorName").value,
            subject: document.getElementById("subjectName").value,
            start: document.getElementById("startTime").value,
            end: document.getElementById("endTime").value,
            date: document.getElementById("scheduleDate").value || new Date().toISOString().split("T")[0]
        };

        // Empty field check (add this)
        if (!data.instructor || !data.subject || !data.start || !data.end || !data.date) {
            alert("Please fill in all fields before adding a schedule.");
            return;
        }

        const currentRoom = roomSelect.value;
        if (!currentRoom) return;


        // conflict check
        const conflictSchedule = schedules[currentRoom].find(s => {
            if (s.date !== data.date) return false;
            const newStart = new Date(`${data.date}T${data.start}`);
            const newEnd = new Date(`${data.date}T${data.end}`);
            const existingStart = new Date(`${s.date}T${s.start}`);
            const existingEnd = new Date(`${s.date}T${s.end}`);
            return newStart < existingEnd && newEnd > existingStart;
        });

        if (conflictSchedule) {
            const conflictPopup = document.getElementById("conflictPopup");
            const conflictMessage = document.getElementById("conflictMessage");

            conflictMessage.innerHTML = `
                <b>Schedule Conflict</b><br><br> 
                <u>New schedule:</u><br>
                ${data.instructor} – ${data.subject}, ${data.start} - ${data.end} (${data.date})<br><br>
                <u>Conflicts with existing:</u><br>
                ${conflictSchedule.instructor} – ${conflictSchedule.subject}, ${conflictSchedule.start} - ${conflictSchedule.end} (${conflictSchedule.date})
            `;

            conflictPopup.style.display = "block";
            document.getElementById("conflictClose").onclick = () => conflictPopup.style.display = "none";
            window.onclick = (e) => { if (e.target === conflictPopup) conflictPopup.style.display = "none"; };
            return;
        }

        // duplicate check (exact same details)
        const duplicate = schedules[currentRoom].find(s =>
            s.instructor === data.instructor &&
            s.subject === data.subject &&
            s.start === data.start &&
            s.end === data.end &&
            s.date === data.date
        );

        if (duplicate) {
            alert("This schedule already exists in this room.");
            return;
        }


        // save if no conflict
        schedules[currentRoom].push(data);
        renderSchedules(currentRoom);

    });

    roomCnt.addEventListener("dragover", (e) => {
        e.preventDefault(); // allow drop
    });

    roomCnt.addEventListener("drop", (e) => {
        e.preventDefault();
        const data = JSON.parse(e.dataTransfer.getData("application/json"));

        // Checkfield is empty
        if (!data.instructor || !data.subject || !data.start || !data.end || !data.date) {
            alert("Please fill in all fields before adding a schedule.");
            return;
        }

        const currentRoom = roomSelect.value;
        if (!currentRoom) return;

        // check kung may current sched
        const conflictSchedule = schedules[currentRoom].find(s => {
            if (s.date !== data.date) return false;

            const newStart = new Date(`${data.date}T${data.start}`);
            const newEnd = new Date(`${data.date}T${data.end}`);
            const existingStart = new Date(`${s.date}T${s.start}`);
            const existingEnd = new Date(`${s.date}T${s.end}`);

            return newStart < existingEnd && newEnd > existingStart;
        });

        if (conflictSchedule) {
            const conflictPopup = document.getElementById("conflictPopup");
            const conflictMessage = document.getElementById("conflictMessage");

            conflictMessage.innerHTML = `
                <b>Schedule Conflict</b><br><br>
                <u>New schedule:</u><br>
                ${data.instructor} – ${data.subject}, ${data.start} - ${data.end} (${data.date})<br><br>
                <u>Conflicts with existing:</u><br>
                ${conflictSchedule.instructor} – ${conflictSchedule.subject}, ${conflictSchedule.start} - ${conflictSchedule.end} (${conflictSchedule.date})
            `;

            conflictPopup.style.display = "block";
            document.getElementById("conflictClose").onclick = () => conflictPopup.style.display = "none";
            window.onclick = (e) => { if (e.target === conflictPopup) conflictPopup.style.display = "none"; };
            return;
        }

        // duplicate check
        const duplicate = schedules[currentRoom].find(s =>
            s.instructor === data.instructor &&
            s.subject === data.subject &&
            s.start === data.start &&
            s.end === data.end &&
            s.date === data.date
        );

        if (duplicate) {
            alert("This schedule already exists in this room.");
            return;
        }

        // no conf then save
        schedules[currentRoom].push(data);
        renderSchedules(currentRoom);

    });

    const initialRoom = ensureInitialRoomSelected();
    if (initialRoom) {
        const room = rooms[initialRoom];
        if (room) {
            roomTitle.textContent = room.name;
            roomDesc.textContent = room.description;
            roomImg.src = room.image;
            roomImg.alt = room.name;
        }
        renderSchedules(initialRoom);
    }
   
});


// render schedules for one room only
function renderSchedules(roomId) {
    scheduleList.innerHTML = ""; // clear old items

    schedules[roomId].forEach(s => {            // HOVER INFO MESSAGE SA ROOM PIC
        const div = document.createElement("div");
        div.textContent = `${s.instructor} – ${s.subject}, ${s.start} - ${s.end} (${s.date})`;
        div.style.margin = "4px 0";
        div.style.padding = "4px";
        div.style.borderBottom = "1px solid #ddd";
        scheduleList.appendChild(div);
    });
}


                // FROM NOW ON, NEW CODES WITH DIFF FEATURE WILL BE ADDED HERE

function openSubCnt() {
    document.getElementById("subjectsBtnInfo").style.display = "block";
}

function closeSubCnt() {
    document.getElementById("subjectsBtnInfo").style.display = "none";
}

function subjectsBtnInfo() {
    const currentRoom = roomSelect.value;
    const subjectsScheduleList = document.getElementById("subjectsBtnCnt");

    if (!currentRoom) {
        subjectsScheduleList.innerHTML = "<i>Please select a room first.</i>";
    } else if (!schedules[currentRoom] || schedules[currentRoom].length === 0) {
        subjectsScheduleList.innerHTML = "<i>No schedules yet for this room.</i>";
    } else {
        subjectsScheduleList.innerHTML = schedules[currentRoom].map(s => `
            <div style="margin-bottom:10px; padding:6px; border:1px solid #ccc; border-radius:5px;">
                <label><b>Date: </b></label> ${s.date} <br />
                <label><b>Subject: </b></label> ${s.subject} <br />
                <label><b>Instructor: </b></label> ${s.instructor} <br />
                <label><b>Time Coverage: </b></label> ${s.start} - ${s.end}
            </div>
        `).join("");
    }

    document.getElementById("subjectsBtnInfo").style.display = "block";
}

function closeSubCnt() {
    document.getElementById("subjectsBtnInfo").style.display = "none";
}
