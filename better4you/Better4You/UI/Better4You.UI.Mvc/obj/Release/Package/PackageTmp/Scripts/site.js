var tar = {
    constants : {
        responseResult: { Success: 1, Failed: 0 },
        menuTypes: {
            Standart: 108001,
            Vegeterian :108002,
            Special :108003,
            Singular: 108004
        },
        orderStatuses: {            
            InitialState:107001,
            InvoiceSent:107002,
            PastDue:107003,
            Paid:107004,
            OrderSubmitted:107005
        },
        typeHead: { minLength: 3, itemsCount: 8 },
        masks: [
            { type: "phone", mask: "(999) 999-9999" },
            { type: "zipcode", mask: "99999" },
            { type: "year", mask: "9999" }
        ],
        application:{server:"/"}
    },
    options: {
        type: "POST",
        async: true,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        traditional: true
    },
    prepareMasks:function ()
    {
        $.each(tar.constants.masks, function(index, item) {
            $("[mask='" + item.type + "']").mask(item.mask);
        });
    },
    preparePlaceHolders:function () {
        $('input, textarea').placeholder();
    },    
    prepareModalActions: function (actionType, label) {
        if (actionType == "create") {
            //$(".lead.text-info", $(".modal-body")).html("<strong> " + label + "</strong>");
            $("#modalTitle", $(".modal-header")).html(label);
            tar.readOnlyForm(false);
            $("#btnClose").show();
            $("#btnSave").show();
            $("#btnEdit").hide();
            $("#btnDelete").hide();
        }
        else if (actionType == "view") {
            //$(".lead.text-info", $(".modal-body")).html("<strong> " + label + "</strong>");
            $("#modalTitle", $(".modal-header")).html(label);
            tar.readOnlyForm(true);
            $("#btnDelete").show();
            $("#btnEdit").show();
            $("#btnSave").hide();
            $("#btnClose").show();
        }
        else if (actionType == "edit") {
            $("#modalTitle", $(".modal-header")).html(label);
            tar.readOnlyForm(false);
            $("#btnClose").show();
            $("#btnSave").show();
            $("#btnEdit").hide();
            $("#btnDelete").hide();
        }
    },
    getUrl: function (action, controller, jsonData) {
        if (action == null || action == "")
            action = "Home";
        var url = controller + "/" + action;
        for (var key in jsonData) {
            if (key.toUpperCase() == "ID")
                url = url + "/" + jsonData[key];
            else if (jsonData[key].constructor==Array && jsonData[key].length>0)
            {
                if (!(url.indexOf('?') >= 0))
                    url = url + "?";
                for (i = 0; i < jsonData[key].length;i++)
                {
                    url = url + key + "=" + jsonData[key][i] + "&";
                }
            }
            else {
                if (!(url.indexOf('?') >= 0))
                    url = url + "?";
                url = url + key + "=" + jsonData[key] + "&";
            }
        }

        var linkArray =
            [
                window.location.protocol + "/",
                window.location.host,
                //"Better4You.UI.Mvc",
                //"Documents"
            ];

        //url = tar.constants.application.server + url;
        url = linkArray.join("/") + "/" + url;
        return url;
    },
    readOnlyForm:function(param) {
        if (param == true)
            {
            $('input:not([type=button],[type=hidden])').attr('disabled', 'true');
            $('select .selectpicker').prop('disabled', "true");
            
        }
        else{
            $('input:not([type=button],[type=hidden])').removeAttr('disabled');
            $('select .selectpicker').prop('disabled', "false");
        }
    },
    openPopup: function (action, controller, jsonData) {
        var popupDiv = $("#popupWindow");
        if (popupDiv.length == 0) {
            popupDiv = $('<div class="modal hide fade" id="popupWindow" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
                            '<div class="modal-header">'+
                                '<button type="button" class="close" data-dismiss="modal" aria-hidden="true"><i class="icon-remove"></i></button>'+
                                '<h3 id="modalTitle"></h3>' +
                            '</div>'+
                            '<div class="modal-body"/>' +
                        '</div>');
            $("body").append(popupDiv);
        }
        
        $("#modalTitle", $(popupDiv)).html(jsonData.label);

        $.get(tar.getUrl(action, controller, jsonData.url)).success(function (data) {
            $('.modal-body', $(popupDiv)).html(data);
        });
        var $modal = $(popupDiv).modal({
            show: false,
            //backdrop: false,
            keyboard:false
        });
        $modal.modal('show');
    },
    closePopup:function () {
        $("#popupWindow").modal("hide");
    },
    openNewForm: function (action, controller, jsonData) {
        return window.open(tar.getUrl(action, controller, jsonData.url));
    },



    removePopupSubmitBehaviour:function(){
        $('button[type="submit"]',$("#popupForm")).attr('type','button');
    },
    redirect: function(url) {
        window.location = url;
    },
    goTo: function (action, controller, jsonData) {
        debugger;
        var url = tar.getUrl(action, controller, jsonData);
        window.location = url;
    },
    appPath: null,
    callAction: function (controllerName, actionName, data, options, hide) {
        var root = this.appPath;
        if (!root)root = "/";
        
        ///<returns type="generateResponse" />
        try {
            if (hide == undefined || hide == null || hide != true)
                tar.showLoading();

            var result = null;
            
            var extendedOptions = $.extend({},tar.options, options);
            
            var prms = {
                type: extendedOptions.type,// "POST",
                url: root + controllerName + "/" + actionName,
                data: JSON.stringify(data),
                dataType: extendedOptions.dataType,//"json",
                contentType: extendedOptions.contentType,//"application/json; charset=utf-8",
                traditional: extendedOptions.traditional,
                async: extendedOptions.async,
                success: function (d) {
                    try {
                        tar.hideLoading();
                        result = d;
                        result = $.extend({
                            Success: d.Result == tar.constants.responseResult.Success
                        }, d);
                        if (extendedOptions.async) {
                            if (result.Success) {
                                alert(result.Message);
                            } else {
                                alert('Error! ' + result.Message);
                            }
                        }
                    } catch (ex) {
                        alert(ex);
                    }
                }
            };

            if (extendedOptions) {
                $.extend(prms, extendedOptions);
            }
            $.ajax(prms);
            if (!extendedOptions.async)
                tar.hideLoading();
        } catch (e) {
            alert('Error! ' + e);
            tar.hideLoading();
        }
        return result;
    },
    loading: function (show) {
        if (show)
            tar.showLoading();
        else
            tar.hideLoading();
    },
    showLoading: function () {
        if ($('#tarLoadingDiv').length > 0) return;
        var loadDiv = $("<div id='tarLoadingDiv' style='position: absolute;top: 5px;right: 5px;'>" +
            "<a class='btn btn-mini btn-success has-spinner active'>" +
            "<span class='spinner'><i class='icon-spin icon-refresh'></i></span>" +
            "...Loading" +
            "</a>" +
            "</div>");
        $(loadDiv).appendTo(document.body);
        
    },
    hideLoading: function () {
        $('#tarLoadingDiv').remove();
    },
    toServerDate: function (jsonDate) {
        var dateFormat = "MM/dd/yyyy, HH:mm:ss";
        var formattedValue = tar.toDateString(jsonDate, dateFormat);
        var unformattedValue = $.parseDate(formattedValue, dateFormat);
        return unformattedValue;
    },
    toDateString: function (jsonDate, dateFormat) {
        if (IsNull(jsonDate, '') != '') {
            //var someDate = new Date(+jsonDate.replace(/\/Date\((-?\d+)\)\//gi, "$1"));
            var someDate = "";
            if ($.type(jsonDate) === "date") someDate = jsonDate;
            else if ($.type(jsonDate) === "string") someDate = jsonDate;
            else someDate = new Date(+jsonDate.replace(/\/Date\((-?\d+)\)\//gi, "$1"));            
            return moment(someDate).format(dateFormat);
        }
        return "";
    },
    toDate: function (jsonDate, dateFormat) {
        if (IsNull(jsonDate, '') != '') {
            var someDate = new Date(+jsonDate.replace(/\/Date\((-?\d+)\)\//gi, "$1"));
            return someDate.toString();
        }
        return "";
    },
    daysInMonth: function (year, month) {
        month = "00" + month;
        month = month.substr(month.length - 2, 2);
        return moment(year + "-" + month, "YYYY-MM").daysInMonth();
    }
    
};
function IsNull(prmValue, defaultValue) {
    if (prmValue == null || prmValue == "null" || prmValue == undefined || prmValue == "undefined")
        return defaultValue;
    else
        return prmValue;
}

tarControls = {
    parameterMap: function (params,query) {
        
        if (params.data == null)
            params.data = {};
        params.data[params.filter.query.varName] = query;
        if (params.filter.cascade != null)
            $.each(params.filter.cascade, function (itemIndex,item) {
                params.data[item.varName] = $("#" + item.varControl).val();
            });
        return params.data;
    },
    //params.filter => typehead Query
    //params.callBack => typehead Process
    //params.action
    //params.controller
    //params.id
    //params.idHidden
    //params.data
    //params.filter.query
    //params.filter.cascade
    //params.model={primaryKey="Id",listName:"List",columns:[]}
    //params.updaterCallBack
    autoComplete: function (params) {
        return {
            source: function (query, callBack) {
                return tar.callAction(
                    params.controller,
                    params.action,
                    tarControls.parameterMap(params,query),
                    {
                        success: function(d) {
                            tar.hideLoading();
                            if (d.Result != tar.constants.responseResult.Success) alert(d.Message);
                            else {
                                var listArray = d.List;
                                if (params.model!=null && params.model.listName != null)
                                    listArray = d[params.model.listName];
                                var resultList = listArray.map(function (listItem) {
                                    var item = {};
                                    
                                    if (params.model != null) {
                                        var columnArray = new Array();
                                        $.each(params.model.columns, function (columnIndex, column) {
                                            var arrColumn = column.split('.');
                                            
                                            column = arrColumn[arrColumn.length - 1];
                                            var objectItem = listItem;
                                            for (var i = 0; i < arrColumn.length - 1; i++) {
                                                objectItem = objectItem[arrColumn[i]];
                                            }
                                            columnArray.push(objectItem[column]);                                                

                                        });
                                        item = { id: listItem[params.model.primaryKey], name: columnArray.join(" - ") };
                                    } else {
                                        item = { id: listItem.Key, name: listItem.Value };
                                    }
                                    //var item = { id: listItem.Key, name: listItem.Value };
                                    return JSON.stringify(item);
                                });
                                callBack(resultList);
                            }

                        }
                    }
                );
            },
            minLength: tar.constants.typeHead.minLength,
            items: tar.constants.typeHead.itemsCount,
            matcher: function(obj) {
                var item = JSON.parse(obj);
                return ~item.name.toLowerCase().indexOf(this.query.toLowerCase());
            },
            
            sorter: function(items) {
                var beginswith = [], caseSensitive = [], caseInsensitive = [];
                var aItem;
                while (aItem = items.shift()) {
                    var item = JSON.parse(aItem);
                    if (!item.name.toLowerCase().indexOf(this.query.toLowerCase())) beginswith.push(JSON.stringify(item));
                    else if (~item.name.indexOf(this.query)) caseSensitive.push(JSON.stringify(item));
                    else caseInsensitive.push(JSON.stringify(item));
                }

                return beginswith.concat(caseSensitive, caseInsensitive);

            },            
            highlighter: function(obj) {
                var item = JSON.parse(obj);
                var query = this.query.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, '\\$&');
                return item.name.replace(new RegExp('(' + query + ')', 'ig'), function($1, match) {
                    return '<strong>' + match + '</strong>';
                });
            },

            updater: function(obj) {
                var item = JSON.parse(obj);
                var hiddenId = params.id + "Id";
                if (params.idHidden != null && params.idHidden !== "") {
                    hiddenId = params.idHidden;
                }
                var parentObject = $("#" + params.id).parent();
                var hiddenObject = $("#" + hiddenId, parentObject);

                if (hiddenObject.length === 0) {
                    hiddenObject = $('<input type="hidden"></input>').attr('id', hiddenId);
                    parentObject.append(hiddenObject);
                }
                
                $(hiddenObject).val(item.id);
                if (params.updaterCallBack != null)
                    return params.updaterCallBack(item.name);
                return item.name;
            }
        };
    }
}