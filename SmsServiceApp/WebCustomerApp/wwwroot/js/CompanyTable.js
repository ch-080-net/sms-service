
var Vue1 = new Vue({

    el: "#tables",
    data: {
        object: {
            timestorege: []
        }
    },
    methods: {
        sendToServer: function () {

            axios({
                method: 'get',
                url: '/Company/index',
                data: {
                }
            })
                .then(function (Companies) {
                    timestorege = Companies;
                })
                .catch(function (error) {
                    console.log(error);
                });

        }
    }

});

$(document).ready(function () {

    Vue1.methods.sendToServer();
});
