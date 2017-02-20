$(function () {
    // $.ADDLOAD()

    new Vue({
        el: '#main',
        data: {
            info: {}
        },
        ready: function () {
            var _this = this;
            _this.txinfo();
            _this.$nextTick(function () {
                _this.close();
                // $.RMLOAD()
            })
        },
        methods: {
            txinfo: function () {
                var tximg=localStorage.qy_head.split('|')[1];
                console.log(tximg)
                $('.toux img').attr('src',tximg)
            },
            close: function () {
                $('.xx').on('click', function () {
                    window.location.href = '../index.html'
                })
            }
        }
    })
})