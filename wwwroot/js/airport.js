console.log("airport.js loaded ✅");

function showConfirm(id) {
    document.getElementById('ConfirmDeleteSpan_' + id).style.display = 'inline';
    document.getElementById('deletespan_' + id).style.display = 'none';
}

function hideConfirm(id) {
    document.getElementById('ConfirmDeleteSpan_' + id).style.display = 'none';
    document.getElementById('deletespan_' + id).style.display = 'inline';
}
    function filterAirportTable() {
            var searchText = document.getElementById('airportSearchBox').value.toLowerCase();
    var rows = document.querySelectorAll('table tbody tr');

    rows.forEach(function (row) {
        row.style.display = row.textContent.toLowerCase().includes(searchText) ? '' : 'none';
            });
        }

    document.getElementById('searchBtn').addEventListener('click', filterAirportTable);

    document.getElementById('airportSearchBox').addEventListener('keyup', function (e) {
            if (e.key === 'Enter') {
        filterAirportTable();
            }
        });


