
var arr=["Sms service","The Best","Great Performance","Optimized","Quick Access","Protection Of Personal Data"];

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
