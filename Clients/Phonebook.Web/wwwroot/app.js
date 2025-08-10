const contactsApi = window.contactsApi;
const reportsApi = window.reportsApi;
let selectedContactId = null;
let selectedContact = null;

document.getElementById("create-contact-form").addEventListener("submit", async (e) => {
    e.preventDefault();
    const name = document.getElementById("name").value;
    const surname = document.getElementById("surname").value;
    const company = document.getElementById("company").value;

    const res = await fetch(contactsApi, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ name, surname, company }),
    });

    if (res.ok) loadContacts();
});

async function loadContacts() {
    const res = await fetch(contactsApi);
    const data = await res.json();
    const list = document.getElementById("contact-list");
    list.innerHTML = "";

    const label = document.getElementById("selected-contact-label");
    label.textContent = "Select a contact to view its details!";

    data.data.forEach(contact => {
        const li = createContactListItem(contact);
        list.appendChild(li);
    });
}

function createContactListItem(contact) {
    const listItem = document.createElement("li");
    listItem.className = "list-item";

    const info = document.createElement("div");
    info.className = "contact-info";

    const icon = document.createElement("span");
    icon.className = "contact-icon";
    icon.textContent = "📄";

    const text = document.createElement("span");
    text.className = "list-text";
    text.textContent = `${contact.name} ${contact.surname} (${contact.company})`;

    info.appendChild(icon);
    info.appendChild(text);
    listItem.appendChild(info);

    listItem.onclick = () => handleContactSelect(contact, listItem);

    const delBtn = document.createElement("button");
    delBtn.className = "list-button delete";
    delBtn.textContent = "Delete";
    delBtn.onclick = (e) => handleContactDelete(e, contact);

    listItem.appendChild(delBtn);

    return listItem;
}

function handleContactSelect(contact, liElement) {
    selectedContactId = contact.id;
    selectedContact = contact;

    const selectedContactLabel = document.getElementById("selected-contact-label");
    selectedContactLabel.textContent = `For contact: ${contact.name} ${contact.surname} (${contact.company})`;

    loadContactDetails(contact.id);

    document.querySelectorAll("#contact-list .list-item")
        .forEach(el => el.classList.remove("selected"));

    liElement.classList.add("selected");
}

async function handleContactDelete(event, contact) {
    event.stopPropagation();

    const confirmed = confirm(`Are you sure you want to delete ${contact.name} ${contact.surname}?`);
    if (!confirmed) return;

    const deleteRes = await fetch(`${contactsApi}/${contact.id}`, {
        method: "DELETE"
    });

    if (deleteRes.ok) {
        selectedContactId = null;
        selectedContact = null;
        document.getElementById("selected-contact-label").textContent = "";
        document.getElementById("detail-list").innerHTML = "";
        loadContacts();
    } else {
        alert("Failed to delete contact.");
    }
}

document.getElementById("create-detail-form").addEventListener("submit", async (e) => {
    e.preventDefault();
    if (!selectedContactId) return;

    const type = document.getElementById("detail-type").value;
    const value = document.getElementById("detail-value").value;

    await fetch(`${contactsApi}/${selectedContactId}/details`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ type, value }),
    });

    loadContactDetails(selectedContactId);
});

async function loadContactDetails(contactId) {

    const res = await fetch(`${contactsApi}/${contactId}/details`);
    const data = await res.json();
    const list = document.getElementById("detail-list");
    list.innerHTML = "";

    data.data.forEach(detail => {

        const li = document.createElement("li");
        li.className = "list-item";

        const text = document.createElement("span");
        text.className = "list-text";
        text.textContent = `${detail.type}: ${detail.value}`;

        const delBtn = document.createElement("button");
        delBtn.className = "list-button delete";
        delBtn.textContent = "Delete";

        delBtn.onclick = async () => {
            await fetch(`${contactsApi}/${contactId}/details/${detail.id}`, {
                method: "DELETE"
            });
            loadContactDetails(contactId);
        };

        li.appendChild(text);
        li.appendChild(delBtn);
        list.appendChild(li);

    });
}

document.getElementById("request-report").addEventListener("click", async () => {
    await fetch(reportsApi, { method: "POST" });
    loadReports();
});

const reportDetailList = document.getElementById("report-detail-list");

async function loadReports() {

    reportDetailList.style.display = "none";
    reportDetailList.innerHTML = "";

    const res = await fetch(reportsApi);
    const data = await res.json();
    const list = document.getElementById("report-list");
    list.innerHTML = "";

    if (!data.data || data.data.length === 0) {
        const emptyMessage = document.createElement("li");
        emptyMessage.className = "list-item";
        emptyMessage.innerHTML = `<span class="list-text">📭 No reports available.</span>`;
        list.appendChild(emptyMessage);
        return;
    }

    data.data.forEach(report => {

        const li = document.createElement("li");
        li.className = "list-item";

        const text = document.createElement("span");
        text.className = "list-text";

        const formattedDate = new Date(report.requestDate).toLocaleString('tr-TR');
        text.textContent = `📝 Report ${report.id} - ${report.status} (Requested: ${formattedDate})`;

        const viewBtn = document.createElement("button");
        viewBtn.className = "list-button";
        viewBtn.textContent = "View Detail";

        viewBtn.onclick = () => showReportDetail(report.id);

        li.appendChild(text);
        li.appendChild(viewBtn);
        list.appendChild(li);
    });
}

async function showReportDetail(reportId) {

    const reportDetailResult = await fetch(`${reportsApi}/${reportId}/data`);
    const reportDetailResponse = await reportDetailResult.json();

    const reportResult = await fetch(`${reportsApi}/${reportId}`);
    const reportResponse = await reportResult.json();
    const reportData = reportResponse.data;

    const selectedReportLabel = document.getElementById("selected-report-label");
    const formattedDate = new Date(reportData.requestDate).toLocaleString('tr-TR');
    selectedReportLabel.textContent = `For report id: #${reportData.id} (Requested at: ${formattedDate})`;

    reportDetailList.style.display = "block";
    reportDetailList.innerHTML = "";

    const reportDetailData = reportDetailResponse.data;

    if (!reportDetailData || reportDetailData.length === 0) {
        const emptyMessage = document.createElement("li");
        emptyMessage.className = "list-item";
        emptyMessage.innerHTML = `<span class="list-text">📭 No report data available yet.</span>`;
        reportDetailList.appendChild(emptyMessage);
        return;
    }

    reportDetailData.forEach(item => {

        const locationItem = document.createElement("li");
        locationItem.className = "list-item";
        locationItem.innerHTML = `<span class="list-text">📍 Location: ${item.location}</span>`;
        reportDetailList.appendChild(locationItem);

        const contactCountItem = document.createElement("li");
        contactCountItem.className = "list-item";
        contactCountItem.innerHTML = `<span class="list-text">👥 Contacts: ${item.contactCount}</span>`;
        reportDetailList.appendChild(contactCountItem);

        const phoneCountItem = document.createElement("li");
        phoneCountItem.className = "list-item";
        phoneCountItem.innerHTML = `<span class="list-text">📞 Phones: ${item.phoneNumberCount}</span>`;
        reportDetailList.appendChild(phoneCountItem);

        const separator = document.createElement("hr");
        reportDetailList.appendChild(separator);

    });

}

loadContacts();
loadReports();

document.getElementById("selected-report-label").textContent = "Select a report to view details";

document.getElementById("refresh-report").addEventListener("click", () => {
    loadReports();
});
