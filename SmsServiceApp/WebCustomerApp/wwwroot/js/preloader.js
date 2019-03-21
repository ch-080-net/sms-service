
var arr=["Sms service","The Best","Great Performance","Optimized","Quick Access","Protection Data"];

document.body.onload=function(){
    if (sessionStorage.getItem('dontLoad') == null) {
        var preloader = document.getElementById('page_preloader');
        preloader.classList.add('preview');
        var preloadertext = document.getElementById('text_preloader');
        preloadertext.classList.add('loader');
        var i = 0;

        setInterval(() => {
            if (i < arr.length) {
                preloadertext.textContent = arr[i];
                i++;
            }
        }, 750);

        setTimeout(function () {

            if (!preloader.classList.contains('done')) {
                preloader.classList.add('done');
            }
                sessionStorage.setItem('dontLoad', 'true');
        },750 * arr.length + 750);
    }
};
