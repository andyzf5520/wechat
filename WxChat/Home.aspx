﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="WxChat.Home" %>

<!DOCTYPE html>
<html>

<head>

    <meta charset="UTF-8">
    <title>门店注册</title>
    <!--必须-->
    <meta name="viewport" content="width=device-width,initial-scale=1,user-scalable=0">
    <!--引入css样式文件(可以自己从网上下载，也可引用bootcdn里网络资源的样式) 微信官方weui + JqueryWEUI(手机专用前端插件)->-->

    <link rel="stylesheet" type="text/css" href="https://cdn.bootcss.com/weui/1.1.3/style/weui.min.css" />
    <link rel="stylesheet" href="https://cdn.bootcss.com/jquery-weui/1.2.1/css/jquery-weui.min.css" />

    <style>
        .error {
            color: red;
        }

        .weui-input error,
        .weui-input,
        .selects {
            width: 50%;
            padding-right: 3px;
        }

        .weui-label {
            width: 110px;
        }

        .weui-cell {
            padding: 15px;
        }

        .weui-top {
            flex: 1;
            text-align: center;
            /* margin-top: 20px; */
            height: 40px;
            justify-content: center;
            background-color: cadetblue;
            font-size: 20px;
            color: aliceblue;
        }
    </style>


</head>


<body>

    <%--<div class="weui-top">门店注册</div>--%>
    <div class="weui-cells__group weui-cells__group_form">
        <form class="cmxform" runat="server" id="form" method="POST">
            <!--一下两者下拉框必须放在form中不然提交验证不生效-->
            <!-- <div class="weui-cell">
                <div class="weui-cell__hd"><label for="name" class="weui-label">原生下拉框</label></div>
                <select class="weui-select" name="select2">
                    <option value="1">中国</option>
                    <option value="2">美国</option>
                    <option value="3">英国</option>
                  </select>
            </div> -->
            <!-- <div class="weui-cell">
                <div class="weui-cell__hd"><label for="mobile" class="weui-label">手机picker选择器</label></div>
                <div class="weui-cell__bd">
                    <input class="weui-input" id="mobile" required title="输入" type="text" value="">
                </div>
            </div> -->

            <div class="weui-cell">
                <div class="weui-cell__hd">
                    <label for="name" class="weui-label">经营大类</label>
                </div>
                <div class="weui-cell__bd">
                    <input class="weui-input" required title="请选择经营大类" id="businessScope" name="businessScope" type="text" value="">
                </div>
            </div>


            <div class="weui-cell">
                <div class="weui-cell__hd" for="manageBusinessType">
                    <label for="manageBusinessType" class="weui-label">商户类型</label>
                </div>
                <div class="weui-cell__bd">
                    <label for="manageBusinessType" class="s">
                        <input type="radio" checked="checked" id="manageBusinessType" value="0" name="manageBusinessType">
                        批发商
                    </label>
                    <label for="manageBusinessType" class="s">
                        <input type="radio" id="manageBusinessType" value="1" name="manageBusinessType">
                        零售商
                    </label>
                </div>

            </div>
            <div class="weui-cell">
                <div class="weui-cell__hd">
                    <label for="manageNodeId" class="weui-label">所属企业</label>
                </div>
                <div class="weui-cell__bd">
                    <input class="weui-input" id="manageNodeId" name="manageNodeId" type="text" value="">
                </div>
            </div>
            <div class="weui-cell">
                <div class="weui-cell__hd">
                    <label for="ownerMarket" class="weui-label">所属市场</label>
                </div>
                <div class="weui-cell__bd">
                    <input class="weui-input" id="ownerMarket" required title="选择所属市场" name="ownerMarket" type="text" value="">
                </div>
            </div>

            <div class="weui-cell">
                <div class="weui-cell__hd" for="manageUserName">
                    <label class="weui-label">门店名称</label>
                </div>
                <div class="weui-cell__bd">
                    <input id="manageUserName" name="manageUserName" placeholder="请填写门店名称" type="text" class="weui-input" />
                </div>
            </div>
            <!-- <div class="weui-cell" id="ishidden" style="display: none;"> -->
            <div class="weui-cell" id="ishidden">
                <div class="weui-cell__hd">
                    <label for="manageAddress" class="weui-label">门店地址</label>
                </div>
                <div class="weui-cell__bd">
                    <input id="manageAddress" required class="weui-input" name="manageAddress" placeholder="请填写门店地址" />
                </div>
            </div>

            <div class="weui-cell">
                <div class="weui-cell__hd">
                    <label for='managePerson' class="weui-label">负责人姓名</label>
                </div>
                <div class="weui-cell__bd">
                    <input id="managePerson" required class="weui-input" name="managePerson" placeholder="负责人姓名" />
                </div>
            </div>

            <div class="weui-cell">
                <div class="weui-cell__hd">
                    <label for='tel' class="weui-label">负责人电话</label>
                </div>
                <div class="weui-cell__bd">
                    <input id="tel" required class="weui-input" name="tel" placeholder="负责人电话" type="number" pattern="[0-9]*" />
                </div>
            </div>

            <div class="weui-cell">
                <div class="weui-cell__hd">
                    <label for='manageBusinessName' class="weui-label">商户名称</label>
                </div>
                <div class="weui-cell__bd">
                    <input id="manageBusinessName" required class="weui-input" name="manageBusinessName" placeholder="商户名称" type="text" />
                </div>
            </div>
            <div class="weui-cell">
                <div class="weui-cell__hd">
                    <label for='businessNo' class="weui-label">摊位号</label>
                </div>
                <div class="weui-cell__bd">
                    <input class="weui-input" id="businessNo" required type="text" name="businessNo" placeholder="摊位号" type="text" />
                </div>
            </div>

            <div class="weui-btn-area ">
                <!-- <input id="btn" class="weui-btn weui-btn_primary" type="button" onclick="SubmitForm()" value="提交" /> -->
                <input id="btn" class="weui-btn  weui-btn_primary" type="submit" value="提交">
             
            </div>
            <div class="button_sp_area" id="hid" hidden="hidden">
                <!-- <a href="javascript:;" id='showModel' class="weui-btn weui-btn_mini  weui-btn_primary">按钮弹框</a> -->
                <%--<a href="javascript:" id="btnCode" class="weui-btn weui-btn_primary">获取授权code</a>--%>
                <a href="javascript:" id="time" style="display: none" class="weui-btn weui-btn_primary">3</a>秒后跳转授权页面
            </div>

 

        </form>
    </div>



</body>
<script src="https://cdn.bootcss.com/jquery/2.2.4/jquery.js"></script>

<!--引入微信的两个js 微信公众号 JS SDK 接口调用必须使用-->
<script type="text/javascript " src="https://res.wx.qq.com/open/js/jweixin-1.0.0.js "></script>
<!--jqueryWEUI原生和微信Weui作用相同-->
<script src="https://cdn.bootcss.com/jquery-weui/1.2.1/js/jquery-weui.min.js"></script>
<!-- <script src="https://res.wx.qq.com/open/libs/weuijs/1.0.0/weui.min.js "></script> -->
<!-- <script src="https://weui.io/zepto.min.js"></script> 相当于 JQuery使用 -->
<script src="https://cdn.bootcss.com/jquery-validate/1.19.1/jquery.validate.js"></script>
<script src="https://static.runoob.com/assets/jquery-validation-1.14.0/dist/localization/messages_zh.js"></script>

<script type="text/javascript">


    
        var productList = [],
            nodeList = [],
            marketList = [];

        var selectNodeId = '',
            selectProduct = '',
            selectMarket = '';


        $.showLoading();
         setTimeout(function () {
              $.hideLoading();

        }, 1500)
        var isend = 0;

        // 所属大类
        var fun1 = new Promise(function (resolve, reject) {
            $.ajax({
                type: "GET",
                contentType: "application/json; charset=utf-8", //   
                url: "https://circulation.bxsuyuan.com/api/ManageUserApp/GetBaseDataList", //url 
                dataType: "json",
                data: {
                    type: "manageProductType"
                },
                success: function (res) {
                    productList = res;
                    productNames = []
                    res.forEach(x => {
                        productNames.push(x.name)
                    })

                    resolve(isend += 1);
                    $("#businessScope").picker({
                        title: "请选择经营大类",
                        cols: [{
                            textAlign: 'center',
                            values: productNames //["水产", "冻肉", "鲜肉", "牛羊肉", "蔬菜"]
                        }],
                        onChange: function (p, v, dv) {
                            // console.log(p, v, dv);
                            console.log(p.value);
                            var selectName = p.value
                            var m = productList.find(x => {
                                return x.name = selectName
                            })
                            selectProduct = m.id
                            console.log('Name:' + p.value + "id:" + selectProduct);
                        },
                        onClose: function (p, v, d) {
                            console.log("close");
                        }
                    });
                    console.log(res);
                    console.log("----获取成功-------")
                },
                error: function (e) {

                    $("#businessScope").picker({
                        title: "请选择经营大类",
                        cols: [{
                            textAlign: 'center',
                            values: []
                        }],

                    });
                    resolve(e);
                    console.log(e.responseJSON.Message)
                    console.log("获取失败")
                },
                complete: function () {
                    console.log("获取失败")
                }
            });
        })
        //  所属企业
        var fun2 = new Promise(function (resolve, reject) {
            $.ajax({
                type: "GET",
                contentType: "application/json; charset=utf-8", //  所属企业
                url: "https://circulation.bxsuyuan.com/api/ManageNodeApp/GetManageNodeListById", //url GetBaseDataList
                dataType: "json",
                data: {
                    manageNodeId: ''
                },
                success: function (res) {
                    nodeList = res;
                    var nodeNames = []
                    res.forEach(x => {
                        nodeNames.push(x.name)
                    })
                    resolve(isend += 1);
                    $("#manageNodeId").picker({
                        title: "请选择所属企业",
                        cols: [{
                            textAlign: 'center',
                            values: nodeNames
                        }],
                        onChange: function (p, v, dv) {
                            console.log(p.value);
                            var selectName = p.value
                            var m = nodeList.find(x => {
                                return x.name = selectName
                            })
                            selectNodeId = m.value
                            console.log('Name:' + p.value + "id:" + selectNodeId);
                        },
                        onClose: function (p, v, d) {
                            console.log("close");
                        }
                    });
                    console.log(res);
                    console.log("----获取成功-------")
                },
                error: function (e) {
                    $("#manageNodeId").picker({
                        title: "请选择所属企业",
                        cols: [{
                            textAlign: 'center',
                            values: []
                        }],

                    });
                    resolve(e);
                    console.log(e.responseJSON.Message)
                    console.log("获取失败")
                },
                complete: function () {
                    console.log("获取失败")
                }
            });
        })
        //  所属市场
        var fun3 = new Promise(function (resolve, reject) {
            $.ajax({
                type: "GET",
                contentType: "application/json; charset=utf-8",
                url: "https://circulation.bxsuyuan.com/api/ManageUserApp/GetBaseDataList", //url 
                dataType: "json",
                data: {
                    type: "ownerMarket"
                },
                success: function (res) {
                    marketList = res
                    marketNames = []
                    res.forEach(x => {
                        marketNames.push(x.name)
                    })
                    resolve(isend += 1);

                    // $("#ownerMarket").picker("setValue", ["2012", "12", "12"]);
                    $('#ownerMarket').picker({
                        title: "请选择所属市场",
                        cols: [{
                            textAlign: 'center',
                            values: marketNames // ['市场1', '市场2']
                        }],
                        onChange: function (p, v, dv) {
                            console.log(p.value);
                            var selectName = p.value
                            var m = marketList.find(x => {
                                return x.name = selectName
                            })
                            selectMarket = m.id

                            console.log('Name:' + p.value + "id:" + selectMarket);
                        },
                        onClose: function (p, v, d) {
                            //改变的时候触发none会校验失效一直为红
                            // if (p.value == '无市场') {
                            //     $('#ishidden').css('display', 'flex')
                            // } else {
                            //     $('#ishidden').css('display', 'none')
                            // }
                            console.log("close");
                        }
                    })
                    console.log(res);
                    console.log("----获取成功-------")
                },
                error: function (e) {
                    $("#ownerMarket").picker({
                        title: "请选择所属市场",
                        cols: [{
                            textAlign: 'center',
                            values: []
                        }],

                    });

                    resolve(e);
                    console.log(e.responseJSON.Message)
                    console.log("获取失败")
                },
                complete: function () {
                    console.log("获取失败")
                }
            });
        })

       

        Promise.all([fun1, fun2, fun3]).then(function (resolve, reject) {
            //做多个异步操作完成后
            $.hideLoading();
            console.log("加载完成")
        }).catch(err => {

            $.hideLoading();
            console.log("加载失败")
        })


    // picker 选择器 可以配合jqueryValidate使用 select 得校验两边才行
    // $("#mobile").picker({
    //     title: "请选择您的手机",
    //     cols: [{
    //         textAlign: 'center',
    //         values: ['iPhone 4', 'iPhone 4S', 'iPhone 5', 'iPhone 5S', 'iPhone 6', 'iPhone 6 Plus', 'iPad 2', 'iPad Retina', 'iPad Air', 'iPad mini', 'iPad mini 2', 'iPad mini 3']
    //     }],
    //     onChange: function(p, v, dv) {
    //         console.log(p, v, dv);
    //     },
    //     onClose: function(p, v, d) {
    //         console.log("close");
    //     }
    // });
</script>
<script>
    $.validator.setDefaults({
        submitHandler: function () {
            // alert("提交事件!");
        }
    });


    $().ready(function () {


        // 校验提示显示顶部
        // $("#showTooltips").click(function() {
        //     var tel = $('#tel').val();
        //     var code = $('#code').val();
        //     if(!tel || !/1[3|4|5|7|8]\d{9}/.test(tel)) $.toptip('请输入手机号');
        //     else if(!code || !/\d{6}/.test(code)) $.toptip('请输入六位手机验证码');
        //     else $.toptip('提交成功', 'success');
        //   });
        // 弹出框提示

 
        $('#showModel').click(function () {
            elstamp = 2019
            $.modal({
                title: "Hello",
                text: '<div id="' + elstamp + '"></div>',
                buttons: [{
                    text: "确认",
                    className: "default",
                    onClick: function () {
                        $('#' + id).val('');
                        //获取唯一input的值到固定input中
                        $('#' + id).val($('.' + '2019').val().replace(' ', ':').replace(' ', ':'))
                        $('.' + elstamp).remove()

                    }
                },]
            });
        });
        // // Jquey自定义校验方法
        $.validator.addMethod("validateTel", function (value, element) {
            if (!value || !/1[3|4|5|7|8]\d{9}/.test(value)) {
                return false
            } else {
                return true
            }
        }, "请输入正确的手机号");

        $("#form").validate({
            rules: {
                businessScope: {
                    required: true
                },
                manageNodeId: {
                    required: true
                },
                ownerMarket: {
                    required: true
                }, // 在input表单中写了require的时候对应写title 或者直接这样定义 下拉框businessScope 就是直接写
                manageUserName: {
                    required: true,
                    minlength: 0,
                    rangelength: [2, 12]
                },
                manageAddress: {
                    required: true,
                    minlength: 0,
                    rangelength: [2, 32]

                },
                managePerson: {
                    required: true,
                    rangelength: [2, 12],
                    minlength: 0
                },
                tel: {

                    required: true,
                    // number: true, //期望的是true,如果为false则展示提示信息
                    rangelength: [11, 11],
                    validateTel: true //期望的是true,如果为false则展示提示信息 自定义校验方法名返回的结果

                },
                manageBusinessName: {
                    required: true,
                    rangelength: [2, 12],
                    minlength: 0 // 不加这个不校验空

                },
                businessNo: {
                    required: true,
                    rangelength: [2, 12],
                    minlength: 0
                }
            },
            messages: {

                businessScope: {
                    required: '请选择经营大类'
                },
                manageNodeId: {
                    required: '请输入所属企业'
                },
                ownerMarket: {
                    required: '请输入所属市场'
                },
                manageUserName: {
                    required: '请输入门店名',
                    rangelength: '长度2-12个字符'
                },
                manageAddress: {
                    required: '请输入门店地址',
                    rangelength: '长度2-32个字符'
                },
                tel: {
                    required: '请输入11位手机号',
                    // number: '请输入数字类型',
                    rangelength: '请输入11位手机号',

                },
                managePerson: {
                    required: '请输入负责人',
                    rangelength: '长度2-12个字符'
                },
                manageBusinessName: {
                    required: "请输入商户名称",
                    rangelength: '长度2-12个字符'
                },
                businessNo: {
                    required: '请输入摊位号',
                    rangelength: '长度2-12个字符'
                },
            },
            submitHandler: function (form) {
                console.log('验证通过')
                // 加载中....
                // if (!tel || !/1[3|4|5|7|8]\d{9}/.test(datas.tel)) $.toptip('请输入正确的手机号');
                // else $.toptip('注册成功', 'success');

                //$('#form').submit(function (params) {
                var ruls = $("#businessScope").rules();

                var datas = {
                    businessScope: selectProduct, //$('#businessScope').val(),
                    manageBusinessType: $('input:radio:checked').val(),
                    manageNodeId: selectNodeId, // $('#manageNodeId').val(),
                    ownerMarket: selectMarket, //$('#ownerMarket').val(),
                    manageUserName: $("#manageUserName").val(),
                    manageAddress: $("#manageAddress").val(),
                    managePerson: $('#managePerson').val(),
                    tel: $('#tel').val(),
                    manageBusinessName: $('#manageBusinessName').val(),
                    businessNo: $('#businessNo').val()
                }
                var listArryJsons = [datas];
                console.log(listArryJsons)

                $.showLoading();


                setTimeout(function () {
                    $.hideLoading();
                }, 2000)
                $.ajax({
                    //几个参数需要注意一下
                    type: "POST",
                    contentType: "application/json; charset=utf-8", // 响应内容格式application/x-www-form-urlencoded  这个是form表单默认提交方式在参数那边会小时FormData

                    url: "https://circulation.bxsuyuan.com/Api/ManageUserApp/InsertByModels", //url
                    dataType: "json",
                    //data: $('#form').serialize(),
                    data: JSON.stringify(listArryJsons),
                    success: function (res) {
                        // $.toptip('注册成功', 'success');
                        //$.toast("注册成功", function() {
                        //    console.log('close');
                        //});

                        $.confirm({
                            title: '提示',
                            text: '注册成功',
                            onOK: function () {
                                //点击确认
                            },
                            onCancel: function () {

                            }
                        });

                    },
                    error: function (e) {
                        $.toptip(e.responseJSON.Message);

                        console.log(e.responseJSON.Message)
                    },
                    complete: function () {
                        console.log("失败")
                    }
                })



                //})


            }

        })



    });
</script>

</html>
