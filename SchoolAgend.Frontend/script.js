const API_BASE = "http://localhost:5119/api/"; 
let formModal = new bootstrap.Modal(document.getElementById('formModal'));

function showSection(section) {
    document.querySelectorAll('.section').forEach(s => s.style.display = 'none');
    document.getElementById(`${section}-section`).style.display = 'block';
}

function showCourseForm(course = null) {
    document.getElementById("form-title").innerText = course ? "Editar Curso" : "Nuevo Curso";
    const form = document.getElementById("dynamic-form");

    form.innerHTML = `
        <input type="text" class="form-control mb-2" id="course-name" placeholder="Nombre" required>
        <input type="text" class="form-control mb-2" id="course-teacher" placeholder="Profesor" required>
        <input type="color" class="form-control mb-2" id="course-color" value="${course?.color || '#ffffff'}" required>
        <button type="submit" class="btn btn-primary w-100">${course ? "Actualizar" : "Guardar"}</button>
    `;

    if (course) {
        document.getElementById("course-name").value = course.name || "";
        document.getElementById("course-teacher").value = course.teacher || "";
        document.getElementById("course-color").value = course.color || "#ffffff";
        form.dataset.id = course.id;
        form.dataset.rowVersion = course.rowVersion || "";
    } else {
        form.dataset.id = "";
        form.dataset.rowVersion = "";
    }

    form.onsubmit = async (e) => {
        e.preventDefault();
        const name = document.getElementById("course-name").value.trim();
        const teacher = document.getElementById("course-teacher").value.trim();
        const color = document.getElementById("course-color").value;

        if (!name || !teacher || !color) return alert("Todos los campos son obligatorios");

        try {
            let res;
            if (course) {
                const payload = {
                    id: course.id,
                    name,
                    teacher,
                    color,
                    rowVersion: form.dataset.rowVersion
                };
                res = await fetch(`${API_BASE}courses/${course.id}`, {
                    method: "PUT",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify(payload)
                });
            } else {
                const defaultRowVersion = "AAAAAAAAAAA=";
                const payload = { name, teacher, color, rowVersion: defaultRowVersion };
                res = await fetch(`${API_BASE}courses`, {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify(payload)
                });
            }

            if (!res.ok) {
                const msg = await res.text();
                return alert("Error: " + msg);
            }

            formModal.hide();
            loadCourses();
        } catch (err) {
            console.error(err);
            alert("Error al conectarse con el servidor");
        }
    };

    formModal.show();
}

async function loadCourses() {
    const res = await fetch(`${API_BASE}courses`);
    if (!res.ok) return alert("Error al cargar cursos");
    const courses = await res.json();
    const container = document.getElementById("courses-cards");
    container.innerHTML = "";

    courses.forEach(c => {
        const rowVersionBase64 = c.rowVersion || "AAAAAAAAAAA="; 
        c.rowVersion = rowVersionBase64;

        const card = document.createElement("div");
        card.className = "col-md-4";
        card.innerHTML = `
            <div class="card shadow-sm">
                <div class="card-body">
                    <h5 class="card-title">${c.name}</h5>
                    <p class="card-text">Profesor: ${c.teacher}</p>
                    <button class="btn btn-sm btn-primary me-1" onclick='showCourseForm(${JSON.stringify(c)})'>Editar</button>
                    <button class="btn btn-sm btn-danger" onclick='deleteCourse(${c.id})'>Eliminar</button>
                </div>
            </div>
        `;
        container.appendChild(card);
    });
}

async function deleteCourse(id) {
    if (!confirm("¿Eliminar curso?")) return;
    const res = await fetch(`${API_BASE}courses/${id}`, { method: "DELETE" });
    if (!res.ok) return alert("Error al eliminar curso");
    loadCourses();
}
function showTaskForm(task = null) {
    document.getElementById("form-title").innerText = task ? "Editar Tarea" : "Nueva Tarea";
    const form = document.getElementById("dynamic-form");

    form.innerHTML = `
        <input type="text" class="form-control mb-2" id="task-title" placeholder="Título" required>
        <textarea class="form-control mb-2" id="task-desc" placeholder="Descripción"></textarea>
        <input type="datetime-local" class="form-control mb-2" id="task-duedate" required>
        <input type="number" class="form-control mb-2" id="task-courseid" placeholder="ID de Curso" required>
        <button type="submit" class="btn btn-primary w-100">${task ? "Actualizar" : "Guardar"}</button>
    `;

    if (task) {
        document.getElementById("task-title").value = task.title || "";
        document.getElementById("task-desc").value = task.description || "";
        document.getElementById("task-duedate").value = task.dueDate ? task.dueDate.slice(0,16) : "";
        document.getElementById("task-courseid").value = task.courseId || "";
        form.dataset.id = task.id;
        form.dataset.rowVersion = task.rowVersion || ""; 
    } else {
        form.dataset.id = "";
        form.dataset.rowVersion = "";
    }

    form.onsubmit = async (e) => {
        e.preventDefault();
        const title = document.getElementById("task-title").value.trim();
        const description = document.getElementById("task-desc").value.trim();
        const dueDate = document.getElementById("task-duedate").value;
        const courseId = parseInt(document.getElementById("task-courseid").value);

        if (!title || !dueDate || isNaN(courseId)) 
            return alert("Todos los campos obligatorios deben ser válidos");

        const payload = task
            ? { id: task.id, title, description, dueDate, courseId, rowVersion: form.dataset.rowVersion }
            : { title, description, dueDate, courseId };

        try {
            const res = await fetch(`${API_BASE}tasksi`, {
                method: task ? "PUT" : "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payload)
            });

            if (!res.ok) return alert("Error: " + await res.text());

            formModal.hide();
            loadTasks();
        } catch (err) {
            console.error(err);
            alert("Error al conectarse con el servidor");
        }
    };

    formModal.show();
}


async function loadTasks() {
    const res = await fetch(`${API_BASE}tasksi`);
    if (!res.ok) return alert("Error al cargar tareas");
    const tasks = await res.json();

    const container = document.getElementById("tasks-cards");
    container.innerHTML = "";

    tasks.forEach(t => {
        const card = document.createElement("div");
        card.className = "col-md-4";
        card.innerHTML = `
            <div class="card shadow-sm">
                <div class="card-body">
                    <h5 class="card-title">${t.title}</h5>
                    <p class="card-text">${t.description}</p>
                    <p class="card-text"><small>Vence: ${t.dueDate}</small></p>
                    <p class="card-text"><small>Curso ID: ${t.courseId}</small></p>
                    <button class="btn btn-sm btn-primary me-1" onclick='showTaskForm(${JSON.stringify(t)})'>Editar</button>
                    <button class="btn btn-sm btn-danger" onclick='deleteTask(${t.id})'>Eliminar</button>
                </div>
            </div>
        `;
        container.appendChild(card);
    });
}


async function deleteTask(id) {
    if (!confirm("¿Eliminar tarea?")) return;

    const res = await fetch(`${API_BASE}tasksi/${id}`, { method: "DELETE" });
    if (!res.ok) return alert("Error al eliminar tarea");

    loadTasks();
}


function showReminderForm(reminder = null) {
    const form = document.getElementById("dynamic-form");
    document.getElementById("form-title").innerText = reminder ? "Editar Recordatorio" : "Nuevo Recordatorio";

    form.innerHTML = `
        <input type="text" class="form-control mb-2" id="reminder-msg" placeholder="Mensaje" required>
        <input type="datetime-local" class="form-control mb-2" id="reminder-datetime">
        <input type="number" class="form-control mb-2" id="reminder-taskid" placeholder="ID de la Tarea (TaskI)" required>
        <button type="submit" class="btn btn-primary w-100">${reminder ? "Actualizar" : "Guardar"}</button>
    `;

    const defaultDateTime = new Date().toISOString().slice(0, 16); 

    if (reminder) {
        document.getElementById("reminder-msg").value = reminder.message || "";
        document.getElementById("reminder-datetime").value = reminder.reminderDateTime 
            ? reminder.reminderDateTime.slice(0,16) 
            : defaultDateTime;
        document.getElementById("reminder-taskid").value = reminder.taskId || "";
        form.dataset.id = reminder.id;
    } else {
        document.getElementById("reminder-datetime").value = defaultDateTime;
        form.dataset.id = "";
    }

    form.onsubmit = async (e) => {
        e.preventDefault();
        const message = document.getElementById("reminder-msg").value.trim();
        const reminderDateTime = document.getElementById("reminder-datetime").value || defaultDateTime;
        const taskId = parseInt(document.getElementById("reminder-taskid").value);

        if (!message) return alert("El mensaje es obligatorio");
        if (!taskId || taskId <= 0) return alert("Debes ingresar un ID de Tarea válido");

        const payload = reminder
            ? { id: reminder.id, message, reminderDateTime, taskId } 
            : { message, reminderDateTime, taskId };

        try {
            const res = await fetch(`${API_BASE}reminders`, {
                method: reminder ? "PUT" : "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payload)
            });

            if (!res.ok) return alert("Error: " + await res.text());

            formModal.hide();
            loadReminders();
        } catch (err) {
            console.error(err);
            alert("Error al conectarse con el servidor");
        }
    };

    formModal.show();
}

async function loadReminders() {
    const res = await fetch(`${API_BASE}reminders`);
    if (!res.ok) return alert("Error al cargar recordatorios");
    const reminders = await res.json();
    const container = document.getElementById("reminders-cards");
    container.innerHTML = "";
    reminders.forEach(r => {
        const card = document.createElement("div");
        card.className = "col-md-4";
        card.innerHTML = `
            <div class="card shadow-sm">
                <div class="card-body">
                    <p class="card-text">${r.message}</p>
                    <p class="card-text"><small>Vence: ${r.dueDate || r.reminderDateTime}</small></p>
                    <!-- Ya NO mostramos el TaskId aquí -->
                    <button class="btn btn-sm btn-primary me-1" onclick='showReminderForm(${JSON.stringify(r)})'>Editar</button>
                    <button class="btn btn-sm btn-danger" onclick='deleteReminder(${r.id})'>Eliminar</button>
                </div>
            </div>
        `;
        container.appendChild(card);
    });
}


async function deleteReminder(id) {
    if (!confirm("¿Eliminar recordatorio?")) return;
    const res = await fetch(`${API_BASE}reminders/${id}`, { method: "DELETE" });
    if (!res.ok) return alert("Error al eliminar recordatorio");
    loadReminders();
}


function showCourseForm(course = null) {
    document.getElementById("form-title").innerText = course ? "Editar Curso" : "Nuevo Curso";
    const form = document.getElementById("dynamic-form");

    form.innerHTML = `
        <input type="text" class="form-control mb-2" id="course-name" placeholder="Nombre" required>
        <input type="text" class="form-control mb-2" id="course-teacher" placeholder="Profesor" required>
        <input type="color" class="form-control mb-2" id="course-color" value="${course?.color || '#ffffff'}" required>
        <button type="submit" class="btn btn-primary w-100">${course ? "Actualizar" : "Guardar"}</button>
    `;

    if (course) {
        document.getElementById("course-name").value = course.name || "";
        document.getElementById("course-teacher").value = course.teacher || "";
        document.getElementById("course-color").value = course.color || "#ffffff";
        form.dataset.id = course.id;
        form.dataset.rowVersion = course.rowVersion || ""; 
    } else {
        form.dataset.id = "";
        form.dataset.rowVersion = "";
    }

    form.onsubmit = async (e) => {
        e.preventDefault();
        const name = document.getElementById("course-name").value.trim();
        const teacher = document.getElementById("course-teacher").value.trim();
        const color = document.getElementById("course-color").value;

        if (!name || !teacher || !color) return alert("Todos los campos son obligatorios");

        try {
            let res;
            if (course) {
                const payload = {
                    id: course.id,
                    name,
                    teacher,
                    color,
                    rowVersion: form.dataset.rowVersion 
                };
                res = await fetch(`${API_BASE}courses/${course.id}`, {
                    method: "PUT",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify(payload)
                });
            } else {
                const payload = { name, teacher, color };
                res = await fetch(`${API_BASE}courses`, {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify(payload)
                });
            }

            if (!res.ok) {
                const msg = await res.text();
                return alert("Error: " + msg);
            }

            formModal.hide();
            loadCourses();
        } catch (err) {
            console.error(err);
            alert("Error al conectarse con el servidor");
        }
    };

    formModal.show();
}

async function loadCourses() {
    const res = await fetch(`${API_BASE}courses`);
    if (!res.ok) return alert("Error al cargar cursos");
    const courses = await res.json();
    const container = document.getElementById("courses-cards");
    container.innerHTML = "";

    courses.forEach(c => {
        const rowVersionBase64 = c.rowVersion || "AAAAAAAAB9E="; 
        c.rowVersion = rowVersionBase64;

        const card = document.createElement("div");
        card.className = "col-md-4";
        card.innerHTML = `
            <div class="card shadow-sm">
                <div class="card-body">
                    <h5 class="card-title">${c.name}</h5>
                    <p class="card-text">Profesor: ${c.teacher}</p>
                    <button class="btn btn-sm btn-primary me-1" onclick='showCourseForm(${JSON.stringify(c)})'>Editar</button>
                    <button class="btn btn-sm btn-danger" onclick='deleteCourse(${c.id})'>Eliminar</button>
                </div>
            </div>
        `;
        container.appendChild(card);
    });
}

function showSessionForm(session = null) {
    document.getElementById("form-title").innerText = session ? "Editar Sesión" : "Nueva Sesión";
    const form = document.getElementById("dynamic-form");

    form.innerHTML = `
        <input type="text" class="form-control mb-2" id="session-title" placeholder="Título" required>
        <input type="datetime-local" class="form-control mb-2" id="session-start" required>
        <input type="datetime-local" class="form-control mb-2" id="session-end" required>
        <input type="number" class="form-control mb-2" id="session-courseid" placeholder="ID del Curso" required>
        <input type="text" class="form-control mb-2" id="session-classroom" placeholder="Aula">
        <select class="form-control mb-2" id="session-dayofweek" required>
            <option value="1">Lunes</option>
            <option value="2">Martes</option>
            <option value="3">Miércoles</option>
            <option value="4">Jueves</option>
            <option value="5">Viernes</option>
            <option value="6">Sábado</option>
            <option value="7">Domingo</option>
        </select>
        <button type="submit" class="btn btn-primary w-100">${session ? "Actualizar" : "Guardar"}</button>
    `;

    if (session) {
        document.getElementById("session-title").value = session.title || "";
        document.getElementById("session-start").value = session.startTime ? session.startTime.slice(0,16) : "";
        document.getElementById("session-end").value = session.endTime ? session.endTime.slice(0,16) : "";
        document.getElementById("session-courseid").value = session.courseId || "";
        document.getElementById("session-classroom").value = session.classroom || "";
        document.getElementById("session-dayofweek").value = session.dayOfWeek || 1;
        form.dataset.id = session.id;
    } else {
        form.dataset.id = "";
    }

    form.onsubmit = async (e) => {
        e.preventDefault();
        const title = document.getElementById("session-title").value.trim();
        const startTime = document.getElementById("session-start").value;
        const endTime = document.getElementById("session-end").value;
        const courseId = parseInt(document.getElementById("session-courseid").value);
        const classroom = document.getElementById("session-classroom").value.trim();
        const dayOfWeek = parseInt(document.getElementById("session-dayofweek").value);

        if (!title || !startTime || !endTime || isNaN(courseId) || isNaN(dayOfWeek)) {
            return alert("Todos los campos obligatorios deben ser válidos");
        }

        const payload = session
            ? { id: session.id, title, startTime, endTime, courseId, classroom, dayOfWeek }
            : { title, startTime, endTime, courseId, classroom, dayOfWeek };

        try {
            const res = await fetch(`${API_BASE}sessions`, {
                method: session ? "PUT" : "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payload)
            });

            if (!res.ok) return alert("Error: " + await res.text());

            formModal.hide();
            loadSessions();
        } catch (err) {
            console.error(err);
            alert("Error al conectarse con el servidor");
        }
    };

    formModal.show();
}

async function loadSessions() {
    const res = await fetch(`${API_BASE}sessions`);
    if (!res.ok) return alert("Error al cargar sesiones");
    const sessions = await res.json();
    const container = document.getElementById("sessions-cards");
    container.innerHTML = "";

    sessions.forEach(s => {
        const startDate = new Date(s.startTime).toLocaleDateString("es-ES", { day: "2-digit", month: "short" });
        const endDate = new Date(s.endTime).toLocaleDateString("es-ES", { day: "2-digit", month: "short" });

        const dayNames = ["Lunes","Martes","Miércoles","Jueves","Viernes","Sábado","Domingo"];
        const dayName = dayNames[(s.dayOfWeek || 1) - 1];

        const card = document.createElement("div");
        card.className = "col-md-4";
        card.innerHTML = `
            <div class="card shadow-sm">
                <div class="card-body">
                    <h5 class="card-title">Sesión #${s.id} - Curso ${s.courseId}</h5>
                    <p class="card-text"><small>Inicio: ${startDate}</small></p>
                    <p class="card-text"><small>Fin: ${endDate}</small></p>
                    <p class="card-text"><small>Aula: ${s.classroom || "-"}</small></p>
                    <p class="card-text"><small>Día: ${dayName}</small></p>
                    <button class="btn btn-sm btn-primary me-1" onclick='showSessionForm(${JSON.stringify(s)})'>Editar</button>
                    <button class="btn btn-sm btn-danger" onclick='deleteSession(${s.id})'>Eliminar</button>
                </div>
            </div>
        `;
        container.appendChild(card);
    });
}

async function deleteSession(id) {
    if (!confirm("¿Eliminar sesión?")) return;
    const res = await fetch(`${API_BASE}sessions/${id}`, { method: "DELETE" });
    if (!res.ok) return alert("Error al eliminar sesión");
    loadSessions();
}

const dayNames = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
const agendaContainer = document.getElementById("agenda-grid");

function buildAgenda() {
  if (!agendaContainer) return;
  agendaContainer.innerHTML = "";
  dayNames.forEach(day => {
    const col = document.createElement("div");
    col.className = "col border p-2";
    col.id = `day-${day}`;
    col.innerHTML = `<h6 class="text-center fw-bold">${day}</h6>`;
    
    col.addEventListener("dragover", e => e.preventDefault());
    col.addEventListener("drop", e => {
      e.preventDefault();
      const sessionId = e.dataTransfer.getData("text/plain");
      const card = document.querySelector(`[data-session-id='${sessionId}']`);
      if (!card) return;

      col.appendChild(card);

      const newDayOfWeek = dayNames.indexOf(day);
      editSession(sessionId, { dayOfWeek: newDayOfWeek });
    });

    agendaContainer.appendChild(col);
  });
}

function addToAgenda(dayOfWeek, title, startTime, endTime, color = "#0d6efd", sessionId = null) {
  const day = dayNames[dayOfWeek % 7];
  const dayCol = document.getElementById(`day-${day}`);
  if (!dayCol) return;

  const card = document.createElement("div");
  card.className = "card mb-2 shadow-sm";
  card.style.borderLeft = `5px solid ${color}`;
  if (sessionId) card.dataset.sessionId = sessionId;

  card.setAttribute("draggable", "true"); 
  card.addEventListener("dragstart", e => {
    e.dataTransfer.setData("text/plain", sessionId);
  });

  card.innerHTML = `
    <div class="card-body p-2">
      <strong>${title}</strong><br>
      <small>${new Date(startTime).toLocaleTimeString([], {hour: "2-digit", minute: "2-digit"})} - ${new Date(endTime).toLocaleTimeString([], {hour: "2-digit", minute: "2-digit"})}</small>
      <div class="mt-1 text-end">
        <button class="btn btn-sm btn-warning edit-btn">✏️</button>
        <button class="btn btn-sm btn-danger delete-btn">🗑️</button>
      </div>
    </div>
  `;

  card.querySelector(".edit-btn").addEventListener("click", e => {
    e.preventDefault();
    editSessionPrompt(sessionId);
  });

  card.querySelector(".delete-btn").addEventListener("click", e => {
    e.preventDefault();
    deleteSession(sessionId, card);
  });

  dayCol.appendChild(card);
}

async function loadAgenda() {
  buildAgenda();
  try {
    const res = await fetch(`${API_BASE}sessions`);
    if (!res.ok) throw new Error("Error cargando sesiones");
    const sessions = await res.json();

    const coursesRes = await fetch(`${API_BASE}courses`);
    const courses = coursesRes.ok ? await coursesRes.json() : [];

    sessions.forEach(s => {
      const course = courses.find(c => c.id === s.courseId);
      addToAgenda(
        s.dayOfWeek,
        s.title || `Sesión Curso ${s.courseId}`,
        s.startTime,
        s.endTime,
        course?.color || "#0d6efd",
        s.id
      );
    });
  } catch (err) {
    console.error(err);
  }
}

function updateSessionInDOM(session, courseColor = "#0d6efd") {
  const card = document.querySelector(`[data-session-id='${session.id}']`);
  if (!card) {
    addToAgenda(session.dayOfWeek, session.title, session.startTime, session.endTime, courseColor, session.id);
    return;
  }

  const currentDay = card.parentElement.id.replace("day-", "");
  const newDay = dayNames[session.dayOfWeek % 7];

  if (currentDay !== newDay) {
    card.remove();
    addToAgenda(session.dayOfWeek, session.title, session.startTime, session.endTime, courseColor, session.id);
    return;
  }

  card.querySelector("strong").textContent = session.title;
  card.querySelector("small").textContent = `${new Date(session.startTime).toLocaleTimeString([], {hour: "2-digit", minute: "2-digit"})} - ${new Date(session.endTime).toLocaleTimeString([], {hour: "2-digit", minute: "2-digit"})}`;
  card.style.borderLeft = `5px solid ${courseColor}`;
}

async function createSession(newData) {
  try {
    const res = await fetch(`${API_BASE}sessions`, {
      method: "POST",
      headers: {"Content-Type": "application/json"},
      body: JSON.stringify(newData)
    });
    if (!res.ok) throw new Error("Error creando sesión");

    const session = await res.json();
    const courseRes = await fetch(`${API_BASE}courses/${session.courseId}`);
    const course = courseRes.ok ? await courseRes.json() : null;

    addToAgenda(session.dayOfWeek, session.title, session.startTime, session.endTime, course?.color || "#0d6efd", session.id);
  } catch (err) {
    console.error(err);
  }
}

async function editSession(sessionId, updatedData) {
  try {
    const res = await fetch(`${API_BASE}sessions/${sessionId}`, {
      method: "PUT",
      headers: {"Content-Type": "application/json"},
      body: JSON.stringify(updatedData)
    });
    if (!res.ok) throw new Error("Error editando sesión");

    const updatedSession = await res.json();
    const courseRes = await fetch(`${API_BASE}courses/${updatedSession.courseId}`);
    const course = courseRes.ok ? await courseRes.json() : null;

    updateSessionInDOM(updatedSession, course?.color || "#0d6efd");
  } catch (err) {
    console.error(err);
  }
}

async function editSessionPrompt(sessionId) {
  const card = document.querySelector(`[data-session-id='${sessionId}']`);
  const newTitle = prompt("Nuevo título:", card.querySelector("strong").textContent);
  if (!newTitle) return;

  const times = card.querySelector("small").textContent.split(" - ");
  const newStart = prompt("Hora inicio (HH:MM):", times[0]);
  const newEnd = prompt("Hora fin (HH:MM):", times[1]);
  if (!newStart || !newEnd) return;

  editSession(sessionId, {title: newTitle, startTime: newStart, endTime: newEnd});
}

async function deleteSession(sessionId, cardElement) {
  if (!confirm("¿Eliminar sesión?")) return;
  try {
    const res = await fetch(`${API_BASE}sessions/${sessionId}`, { method: "DELETE" });
    if (!res.ok) throw new Error("Error eliminando sesión");

    if (cardElement) cardElement.remove();
  } catch (err) {
    console.error(err);
  }
}

document.addEventListener("DOMContentLoaded", () => loadAgenda());

loadCourses();
loadTasks();
loadReminders();
loadSessions();

