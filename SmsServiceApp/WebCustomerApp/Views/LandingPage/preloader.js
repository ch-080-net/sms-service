
var arr=["Sms service","The Best","Great Performance","Optimized","Quick Access","Protection Of Personal Data","на правах реклами 579 з мобільного","Тут може бути ваша реклама   :D"];

document.body.onload=function(){
    var preloader=document.getElementById('page_preloader');
    var preloadertext=document.getElementById('text_preloader');
       
var i=0;
 
        setInterval(() => {
            if(i<arr.length){
                preloadertext.textContent=arr[i];
                i++;
                }
        }, 500);
        
    setTimeout(function(){
    
        if(!preloader.classList.contains('done')){
         preloader.classList.add('done');
        }
    },500*arr.length+500);
};
