$(function(){
	$.ADDLOAD();
	$.checkuser();
	new Vue({
		el:'#main',
		data:{
			info:[],
			choose : true,
			chooseList:[
				{
					name : '可用',
					id : true
				},
				{
					name : '不可用',
					id : false
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
				_this.infoajax();
			},
			infoajax:function () {
				var _this=this;
				$.ajax({
					url: '/Api/v1/Coupon',
					type: 'get',
					dataType:'json',
					data:{
						isAvailable:_this.choose
					}
				}).done(function (rs) {
					if (rs.returnCode == 200) {
						_this.info=rs.data.Coupons;
						$.RMLOAD()
					}
				})

			}
		}
	})

})