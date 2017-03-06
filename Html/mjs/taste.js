$(function(){
    var vm = new Vue({
        el:'#main',
        data:{
            choose : 1,
            chooseList : [
                {
                    name : '巧克力蛋糕',
                    id : '1'
                },
                {
                    name : '奶油蛋糕',
                    id : '2'
                },
                {
                    name : '水果蛋糕',
                    id : '3'
                },
                {
                    name : '蛋糕',
                    id : '4'
                }
            ]
        },
        methods:{
            chooseAction:function(id){
                this.choose = id;
            }
        }
    })

})