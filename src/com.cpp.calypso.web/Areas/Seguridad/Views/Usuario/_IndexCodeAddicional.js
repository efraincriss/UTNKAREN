(function ($) {

    var _$modal = $('#mod-confirmar-rest');

    $('.nuc_action_Reseteo').click(function (e) {

        e.preventDefault();

        var entityId = $(this).attr("data-entity-id");

        _$modal.modal('show');
       
        _$modal.on('show.bs.modal', function (event) {
  
            // Update the modal's content.
            var modal = $(this)
            modal.find('#mod-confirmar-rest-entity-id').val(entityId)
        })
    });

    _$modal.find('#btn-reset-no').click(function (e) {
        _$modal.modal('hide');
    });

    _$modal.find('#btn-reset-si').click(function (e) {
        e.preventDefault();

        var entityId = $("#mod-confirmar-rest-entity-id").val();
        var data = { Id : entityId};
        


        $.ajax({
            url: abp.appPath + 'Seguridad/Usuario/Reseteo',
            data: data,
            type: 'POST',
            success: function (response) {

                if (response.success === true) {

                    _$modal.modal('hide');

                    toastr["success"]("Proceso guardado exitosamente");

                } else if (response.success === false) {

                    var message = $.fn.responseAjaxErrorToString(response);           
                    toastr["error"](message);

                } else { //not wrapped result

                    toastr["error"](response);
                }
            }
        });
    });

})(jQuery);