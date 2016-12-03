$(function () {
    $('#modal-container').on('hidden.bs.modal', function () {
        $(this).removeData('bs.modal');
        $('#modal-container .modal-content').empty();
    });
});