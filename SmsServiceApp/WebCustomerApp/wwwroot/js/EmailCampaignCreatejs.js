var vue = new Vue({
    el: '#app',
    data: () => ({
        dialog: false,
        step: 1,
        active: null,
        firstStepComplite: false,
        secondStepComplite: false,
        thirdStepComplite: false,
        index: 0,
        Campaign: {
            emailAddress: "",
            name: "",
            message: "",
            description: "",
            sendingTime: "",
            recipientsCount: 0,
            sendNow: "true"
        },
        Recepients: [],
        Recepient: {
            Id: null,
            EmailAddress: null,
            Name: null,
            Surname: null,
            BirthDate: null,
            Gender: null,
            Priority: null,
            KeyWords: null
        },
        Gender: ["Male", "Female"],
        Priority: ["Low", "Medium", "High"]
    }),
    methods: {
        submit() {
            if (this.firstStepComplite && this.secondStepComplite && this.thirdStepComplite) {
                $.ajax({
                    url: '/EmailCampaign/CreateCampaign/',
                    type: 'POST',
                    data: {
                        campaign: this.Campaign,
                        recepients: this.Recepients
                    },
                    success: function (href) {
                        window.location.href = href.newUrl;
                    },
                    error: function (request, message, error) {
                        handleException(request, message, error);
                    }
                });
            }
            else {
                alert("To submit complite all steps");
            }
        },
        FirstStep() {
            this.Campaign.sendingTime = document.getElementById('result').value;
            var re = /^(([^<>()\[\]\.,;:\s@\"]+(\.[^<>()\[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;
            if (this.Campaign.name == "" || this.Campaign.emailAddress == "" ||
                (this.Campaign.sendingTime == "" && !this.Campaign.sendNow) || !re.test(this.Campaign.emailAddress)) {
                if (this.Campaign.name == "")
                    $("#NameValidationError").text("Name field is required");
                else
                    $("#NameValidationError").text("");
                if (this.Campaign.emailAddress == "")
                    $("#EmailValidationError").text("Email field is required");
                else {
                    if (!re.test(this.Campaign.emailAddress))
                        $("#EmailValidationError").text("Invalid email");
                    else
                        $("#EmailValidationError").text("");
                }
                if (this.Campaign.sendingTime == "")
                    $("#SendTimeValidationError").text("Time of send field is required");
                else
                    $("#SendTimeValidationError").text("");
                this.firstStepComplite = false;
            }
            else {
                this.step = 2;
                $("#NameValidationError").text("");
                $("#EmailValidationError").text("");
                $("#SendTimeValidationError").text("");
                if (this.Campaign.sendNow == "true")
                    this.Campaign.sendingTime = "now";
                this.firstStepComplite = true;
            }
        },
        SecondStep() {
            this.Campaign.message = tinymce.get('myTextarea').getContent();
            console.log(this.Campaign.message);
            this.step = 3;
            this.secondStepComplite = true;
        },
        ThirdStep() {
            this.step = 4;
            this.thirdStepComplite = true;
        },
        AddRecepient() {
            var valid = true;
            var re = /^(([^<>()\[\]\.,;:\s@\"]+(\.[^<>()\[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;
            if (this.Recepient.emailAddress == "") {
                $("#RecepientEmailValidationError").text("Email field is required");
                return;
            }
            else {
                if (!re.test(this.Recepient.EmailAddress)) {
                    $("#RecepientEmailValidationError").text("Invalid email");
                    return;
                }
                else
                    $("#RecepientEmailValidationError").text("");
            }
            var found = false;
            for (var i = 0; i < this.Recepients.length; i++) {
                if (this.Recepients[i].EmailAddress == this.Recepient.EmailAddress) {
                    found = true;
                    break;
                }
            }
            if (found) {
                $("#RecepientEmailValidationError").text("Recepient with this email already exist");
                return;
            }
            this.Recepient.BirthDate = $("#birthDate").val();
            this.index++;
            this.Recepient.Id = this.index;
            this.Campaign.recipientsCount++;
            this.Recepients.push(Object.assign({}, this.Recepient));
            this.Recepient.Id = null;
            this.Recepient.EmailAddress = null;
            this.Recepient.Name = null;
            this.Recepient.Surname = null;
            this.Recepient.BirthDate = null;
            this.Recepient.Gender = null;
            this.Recepient.Priority = null;
            this.Recepient.KeyWords = null;
        },
        DeleteRecepient(event) {
            var recid = $(event.target).data("recid");
            this.Recepients.splice(recid - 1, 1);
            this.index--;
        },
        closeDialog() {
            this.dialog = false;
        },
        openDialog() {
            this.dialog = true;
        },
        GetFromFile() {
            var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.csv|.txt)$/;
            if (regex.test($("#fileUpload").val().toLowerCase())) {
                if (typeof (FileReader) != "undefined") {
                    var reader = new FileReader();
                    reader.readAsText($("#fileUpload")[0].files[0]);

                    reader.onload = function () {
                        var recepientrow = reader.result;
                        var lines = recepientrow.split("\n");
                        var result = [];
                        var headers = lines[0].split(",");
                        for (var i = 1; i < lines.length; i++) {

                            var obj = {};
                            var currentline = lines[i].split(",");

                            for (var j = 0; j < headers.length; j++) {
                                obj[headers[j]] = currentline[j];
                            }

                            result.push(obj);
                            vue.AddRecepientFromFile(obj)
                        }
                        console.dir(result);
                    }
                }
                else {
                    alert("This browser does not support HTML5.");
                }
            }
            else {
                alert("Please upload a valid CSV file.");
            }
        },
        AddRecepientFromFile(recepient) {
            this.Recepients.push(recepient);
            this.Campaign.recipientsCount++;
        }
    }
})