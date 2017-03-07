$(function(){
	$.checkuser();
	new Vue({
		el:'#main',
		data:{
			info:[],
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
		ready:function(){
			var _this=this;
			_this.infoajax();
		},
		methods:{
			chooseAction:function(id){
				var _this=this;
				_this.choose = id;
			},
			infoajax:function () {
				var _this=this;
				$.ajax({
					url: '/Api/v1/Coupon',
					type: 'get',
					dataType:'json',
					data:{
						isAvailable:0
					}
				}).done(function (rs) {
					if (rs.returnCode == 200) {
						_this.info=rs.data;
					}
				})

			}
		}
	})

})