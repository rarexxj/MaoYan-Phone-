$(function () {
    // $.checkuser();
    var id=$.getUrlParam('id');
    var gid = $.getUrlParam('gid');
    var type = $.getUrlParam('type');
    var mode = $.getUrlParam('mode');
    new Vue({
        el: '#shop',
        data: {
            info: {},
            nowinfo: {},
            kaiguan: false
        },
        ready: function () {
            var _this = this;
            _this.infoajax();
            _this.$nextTick(function () {
                _this.submit();
                // $.RMLOAD()
            })
        },
        methods: {
            infoajax: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Merchant/GetAllMerchant',
                    type: 'get'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data;
                        _this.pimg();

                    }
                })
            },
            pimg: function () {
                var _this = this;
                var map = new BMap.Map("allmap");
                var j = [];
                // var mapStyle = {
                //     features: ["road", "building", "water", "land"],//隐藏地图上的poi
                //     style: "dark"  //设置地图风格为高端黑
                // }
                var mapStyle={
                    styleJson:[
                        {
                            "featureType": "all",
                            "elementType": "all",
                            "stylers": {
                                "color": "#000000"
                            }
                        },
                        {
                            "featureType": "arterial",
                            "elementType": "geometry",
                            "stylers": {
                                "color": "#444444",
                                "hue": "#000000",
                                "lightness": -46,
                                "visibility": "on"
                            }
                        },
                        {
                            "featureType": "boundary",
                            "elementType": "all",
                            "stylers": {
                                "color": "#444444",
                                "lightness": -62,
                                "visibility": "on"
                            }
                        },
                        {
                            "featureType": "poi",
                            "elementType": "labels.text.fill",
                            "stylers": {
                                "color": "#ffffff",
                                "lightness": -60,
                                "visibility": "on"
                            }
                        },
                        {
                            "featureType": "label",
                            "elementType": "labels.text.fill",
                            "stylers": {
                                "color": "#ffffff",
                                "lightness": -60
                            }
                        }
                    ]
                }
                map.setMapStyle(mapStyle);


                for (i = 0; i < _this.info.length; i++) {
                    var c = {
                        lat: _this.info[i].Latitude,
                        lng: _this.info[i].Longitude,
                        info: _this.info[i]
                    };
                    // c.push(_this.info[i].Longitude);
                    // c.push(_this.info[i].Latitude);
                    j.push(c);
                    // a.push(_this.info[i].Longitude);
                    // b.push(_this.info[i].Latitude);
                }

                var json_data = j;
                console.log(json_data);
                var pointArray = new Array();
                for (var i = 0; i < json_data.length; i++) {
                    var myIcon = new BMap.Icon("/Html/css/img/logo59.png", new BMap.Size(60, 60));
                    var marker = new BMap.Marker(new BMap.Point(json_data[i].lng, json_data[i].lat), {icon: myIcon}); // 创建点
                    map.addOverlay(marker);
                    marker['datas'] = json_data[i];//增加点
                    pointArray[i] = new BMap.Point(json_data[i].lng, json_data[i].lat);
                    marker.addEventListener("click", attribute);
                }
                //让所有点在视野范围内
                map.setViewport(pointArray);
                //获取覆盖物位置
                function attribute(e) {
                    var $this = this;
                    _this.nowinfo = $this.datas.info;
                    _this.kaiguan = true
                }

                //定位
                var geolocation = new BMap.Geolocation();
                geolocation.getCurrentPosition(function (r) {
                    if (this.getStatus() == BMAP_STATUS_SUCCESS) {
                        var myIcon = new BMap.Icon("/Html/css/img/logo60.png", new BMap.Size(60, 60));
                        var mk = new BMap.Marker(r.point, {icon: myIcon});
                        map.addOverlay(mk);
                        map.panTo(r.point);
                        map.centerAndZoom(r.point, 16);
                    }
                    else {
                        alert('failed' + this.getStatus());
                    }
                }, {enableHighAccuracy: true})

            },
            submit:function () {
                var _this=this;
                $('.submit').on('click',function () {
                    if(id){
                        window.location.href='/Html/nearbyShop.html?shopid='+_this.nowinfo.Id+'&id='+id+'&type='+type+'&mode='+mode
                    }else{
                        window.location.href='/Html/nearbyShop.html?shopid='+_this.nowinfo.Id+'&gid='+gid+'&type='+type+'&mode='+mode
                    }

                })
            }
        }
    })
})