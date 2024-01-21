function SetLimit() {
    const num = document.getElementById("limitInput").value || 50;

    window.location = "https://localhost:7206/Numbers/Limit?num=" + num;
}