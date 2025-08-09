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

    data.data.forEach(contact => {

        const li = document.createElement("li");
        li.className = "list-item";

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
        li.appendChild(info);

        li.onclick = () => {
            selectedContactId = contact.id;
            selectedContact = contact;

            const label = document.getElementById("selected-contact-label");
            label.textContent = `For contact: ${contact.name} ${contact.surname} (${contact.company})`;

            loadDetails(contact.id);

            document.querySelectorAll("#contact-list .list-item")
                .forEach(el => el.classList.remove("selected"));

            li.classList.add("selected");
        };

        list.appendChild(li);

    });
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

    loadDetails(selectedContactId);
});

async function loadDetails(contactId) {

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
            loadDetails(contactId);
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

        const formattedDate = new Date(report.requestDate).toLocaleString('tr-TR'); // or 'en-US' if preferred
        text.textContent = `📝 Report ${report.id} - ${report.status ?? "Unknown"} (Requested: ${formattedDate})`;

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

    const detailRes = await fetch(`${reportsApi}/${reportId}/data`);
    const detailData = await detailRes.json();

    const reportRes = await fetch(`${reportsApi}/${reportId}`);
    const reportMeta = await reportRes.json();
    const report = reportMeta.data;

    const label = document.getElementById("selected-report-label");
    const formattedDate = new Date(report.requestDate).toLocaleString('tr-TR');
    label.textContent = `For report id: #${report.id} (Requested at: ${formattedDate})`;

    reportDetailList.style.display = "block";
    reportDetailList.innerHTML = "";

    const details = detailData.data;

    if (!details || details.length === 0) {
        const emptyMessage = document.createElement("li");
        emptyMessage.className = "list-item";
        emptyMessage.innerHTML = `<span class="list-text">📭 No report data available yet.</span>`;
        reportDetailList.appendChild(emptyMessage);
        return;
    }

    details.forEach(item => {

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
