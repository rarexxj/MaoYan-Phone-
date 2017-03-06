$(function(){

	var vm = new Vue({
		el:'#main',
		data:{
			choose : 0,
			chooseList:[
				{
					name : '可用',
					id : 0
				},
				{
					name : '不可用',
					id : 1
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