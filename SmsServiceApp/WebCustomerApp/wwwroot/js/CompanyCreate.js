 $(document).ready(function () {
            /*  Activate the tooltips      */
            $('[rel="tooltip"]').tooltip();

            var navListItems = $('div.setup-panel div a'),
                allWells = $('.setup-content'),
                allNextBtn = $('.nextBtn');

            allWells.hide();

            navListItems.click(function (e) {
                e.preventDefault();
                var $target = $($(this).attr('href')),
                    $item = $(this);

                if (!$item.hasClass('disabled')) {
                    navListItems.removeClass('btn-primary').addClass('btn-default');
                    $item.addClass('btn-primary');
                    allWells.hide();
                    $target.show();
                    $target.find('input:eq(0)').focus();
                }
            });

            allNextBtn.click(function () {
                var curStep = $(this).closest(".setup-content"),
                    curStepBtn = curStep.attr("id"),
                    nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a"),
                    curInputs = curStep.find("input[type='text'],input[type='url']");
               
        

                $(".form-group").removeClass("has-error");
                for (var i = 0; i < curInputs.length; i++) {
                    if (!curInputs[i].validity.valid) {
                        isValid = false;
                        $(curInputs[i]).closest(".form-group").addClass("has-error");
                    }
                }

           
            });

            $('div.setup-panel div a.btn-primary').trigger('click');

            $('[data-toggle="wizard-radio"]').click(function () {
                wizard = $(this).closest('.wizard-card');
                wizard.find('[data-toggle="wizard-radio"]').removeClass('active');
                $(this).addClass('active');
                $(wizard).find('[type="radio"]').removeAttr('checked');
                $(this).find('[type="radio"][name="path"]').prop('checked', true);

            });
           
});
$('#submit_first').click(function () {

    var value = parseInt($("input[name='path']:checked").val());
    console.log(value);
    switch (value) {
        case 0:
            $(".recievestep").css('display', 'none');

            break;
        case 1:
            $('#step-1-1').show();
            break;
        case 2:
            $('#step1-3').show();
            break;

        default:

    }
});