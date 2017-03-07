$(function(){
	$.checkuser();
	var id=$.get_user('Id');
	var vm = new Vue({
		el:'#main',
		data:{
			info:[]
		},
		ready:function(){
			var _this=this;
			_this.infoajax();
			_this.lingqu();
		},
		methods:{
			infoajax:function () {
				var _this=this;
				$.ajax({
					url: '/Api/v1/CouponList',
					type: 'get',
					dataType:'json'
				}).done(function (rs) {
					if (rs.returnCode == 200) {
						_this.info=rs.data;
					}
				})

			},
			lingqu:function () {
				$('#main').on('click','.lingqu',function () {
					var code=$(this).parents().attr('data-id');
					var type=$(this).parents().attr('data-type');
					$.ajax({
						url:'/Api/v1/ReceiveCoupon/'+id,
						type:'post',
						dataType:'json',
						data:{
							Code:code,
							Type:type
						}
					}).done(function (rs) {
						if(rs.returnCode==200){
							$.oppo('领取成功',1);
							setTimeout(function () {
								// window.location.href='/Html/html/personalcenter/getcoupon.html'
							},1000)
						}
					})
				})
			}
		}
	})

})