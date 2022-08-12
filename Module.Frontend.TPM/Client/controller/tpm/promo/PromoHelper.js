Ext.define('App.controller.tpm.promo.PromoHelper', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.tpm.promo.Promo'],

    init: function () {
        this.listen({
            component: {
                //promo mass approval
                'promo #massapprovalbutton': {
                    click: this.onMassApprovalButtonClick
                },
                // promo activity
                'promoactivity #promoActivity_step2 #exportAllPromoProducts': {
                    click: this.onActivityExportPromoProductsClick
                },
                'promoeditorcustom #btn_resetPromo': {
                    click: this.resetPromo
                },
                'promo #canchangeresponsible': {
                    afterrender: this.onChangeResposibleButtonAfterRender,
                    click: this.onChangeResponsible
                },

                'promoeditorcustom': {
                    fillWFPanel: this.fillWorkFlowPanel
                }
            }
        });
    },

    onMassApprovalButtonClick: function (button) {
        var me = this;
        var panel = button.up('panel');
        Ext.Msg.show({
            title: l10n.ns('tpm', 'text').value('Confirmation'),
            msg: l10n.ns('tpm', 'Promo').value('Confirm Approval'),
            fn: function (btn) {
                if (btn === 'yes') {
                    me.onOkMassApprovalClick(btn, me, panel);
                }
            },
            icon: Ext.Msg.QUESTION,
            buttons: Ext.Msg.YESNO,
            buttonText: {
                yes: l10n.ns('tpm', 'button').value('confirm'),
                no: l10n.ns('tpm', 'button').value('cancel')
            }
        });
    },

    onOkMassApprovalClick: function (button, me, window) {
        window.setLoading(true);
        var params = this.getParamsForMassApprove();
        if (params.end >= 0) {
            if (!params.store.data.hasRange(params.start, params.end)) {
                params.store.on('guaranteedrange', this.onFullLoadedStore, this);
                //Заставляем догрузить недостающие записи
                params.store.getRange(params.start, params.end);
            } else {
                this.onFullLoadedStore();
            }
        } else {
            window.setLoading(false);
            App.Notify.pushError("There is no promo to approve!");
        }
    },

    onFullLoadedStore: function () {
        var params = this.getParamsForMassApprove();
        params.store.removeListener('guaranteedrange', this.onFullLoadedStore, this);
        var records = params.store.getRange(params.start, params.end);
        var promoNumbers = "";
        records.forEach(function (el) {
            promoNumbers += el.data.Number + ",";
        })

        var window = Ext.ComponentQuery.query('#massapprovalbutton')[0].up('panel');
        if (promoNumbers != "") {
            $.ajax({
                dataType: 'json',
                url: '/odata/Promoes/MassApprove',
                type: 'POST',
                data: promoNumbers,
                success: function (response) {
                    var data = Ext.JSON.decode(response.value);
                    if (data.success) {
                        window.setLoading(false);
                        App.Notify.pushInfo('Mass approve task created successfully');
                        App.System.openUserTasksPanel();
                    }
                    else {
                        window.setLoading(false);
                        App.Notify.pushError(l10n.ns('tpm', 'text').value('failedStatusLoad'));
                    }
                },
                error: function (data) {
                    window.setLoading(false);
                    App.Notify.pushError(data.responseJSON["odata.error"].innererror.message);
                }
            });
        } else {
            window.setLoading(false);
            App.Notify.pushError("There is no promo to approve!");
        }
    },

    getParamsForMassApprove: function () {
        var result = new Object();
        var grid = Ext.ComponentQuery.query('#promoGrid')[0].down('grid');
        result.store = grid.getStore();
        result.start = 0;
        result.end = result.store.getTotalCount() - 1;
        return result;
    },

    onChangeResposibleButtonAfterRender: function (button) {

        button.setDisabled(true);
    },

    onChangeResponsible: function (button) {
        var grid = this.getGridByButton(button),
            panel = grid.up('combineddirectorypanel'),
            selModel = grid.getSelectionModel();
        var record = selModel.getSelection()[0];
        if (record) {
            var grid = Ext.widget('userrolepromo').down('directorygrid'),
                panel = grid.up('combineddirectorypanel'),
                model = panel.getBaseModel(),
                viewClassName = 'App.view.tpm.promo.UserRolePromo';

            var window = Ext.widget('selectorwindow', {
                items: Ext.create(viewClassName, {
                    baseModel: model
                })
            });
            window.down('#select').promoId = record.data.Id;
            window.down('#select').on('click', this.onSelectButtonClick, this);
            //window.down('#select').setDisabled(false);
            window.show();
        } else {
            App.Notify.pushError(l10n.ns('tpm', 'text').value('failedLoadData'));
        }
    },

    onSelectButtonClick: function (button) {
        promoId = button.promoId;
        var window = button.up('selectorwindow');
        selModel = window.down('directorygrid').getSelectionModel().getSelection()[0];
        if (selModel) {
            var userName = selModel.data.Name;
            window.setLoading(l10n.ns('core').value('savingText'));
            $.ajax({
                dataType: 'json',
                url: '/odata/Promoes/ChangeResponsible?promoId=' + promoId + '&userName=' + userName,
                type: 'POST',
                success: function (response) {
                    var data = Ext.JSON.decode(response.value);
                    if (data.success) {
                        window.setLoading(false);
                        window.close();
                    }
                    else {
                        window.setLoading(false);
                        App.Notify.pushError(l10n.ns('tpm', 'text').value('failedLoadData'));
                    }

                },
                error: function (data) {
                    window.setLoading(false);
                    App.Notify.pushError(data.responseJSON["odata.error"].innererror.message);
                }
            });
        } else {
            App.Notify.pushError(l10n.ns('tpm', 'text').value('failedLoadData'));
        }
    },

    resetPromo: function (btn) {
        var window = btn.up('promoeditorcustom');
        var record = this.getRecord(window);
        var promoId = record.get('Id');
        var promoController = App.app.getController('tpm.promo.Promo');
        var me = promoController;

        window.setLoading(l10n.ns('core').value('savingText'));

        $.ajax({
            dataType: 'json',
            url: '/odata/Promoes/ResetPromo?promoId=' + promoId,
            type: 'POST',
            success: function (response) {
                var data = Ext.JSON.decode(response.value);
                if (data.success) {

                    // window.setLoading(false);
                    App.model.tpm.promo.Promo.load(promoId, {
                        callback: function (newModel, operation) {
                            var grid = Ext.ComponentQuery.query('#promoGrid')[0];
                            var directorygrid = grid ? grid.down('directorygrid') : null;
                            window.promoId = data.Id;
                            window.model = newModel;
                            var panel = Ext.ComponentQuery.query('promoeditorcustom')[0].down('#promoBudgets_step1');



                            var shopperTi = panel.down('numberfield[name=PlanPromoTIShopper]').setValue(null);
                            var marketingTi = panel.down('numberfield[name=PlanPromoTIMarketing]').setValue(null);
                            var branding = panel.down('numberfield[name=PlanPromoBranding]').setValue(null);
                            var costProduction = panel.down('numberfield[name=PlanPromoCostProduction]').setValue(null);

                            var actualPromoTIShopper = panel.down('numberfield[name=ActualPromoTIShopper]').setValue(null);
                            var actualPromoTIMarketing = panel.down('numberfield[name=ActualPromoTIMarketing]').setValue(null);
                            var actualPromoBranding = panel.down('numberfield[name=ActualPromoBranding]').setValue(null);
                            var factCostProduction = panel.down('numberfield[name=ActualPromoCostProduction]').setValue(null);

                            var factTotalCost = panel.down('numberfield[name=ActualPromoCost]').setValue(null);
                            var totalCost = panel.down('numberfield[name=PlanPromoCost]').setValue(null);
                            var factBtl = panel.down('numberfield[name=ActualPromoBTL]').setValue(null);

                            var btl = panel.down('numberfield[name=PlanPromoBTL]').setValue(null);

                            btl.originValue = 0;
                            factBtl.originValue = 0;
                            shopperTi.originValue = 0;
                            marketingTi.originValue = 0;
                            branding.originValue = 0;
                            costProduction.originValue = 0;

                            actualPromoTIShopper.originValue = 0;
                            actualPromoTIMarketing.originValue = 0;
                            actualPromoBranding.originValue = 0;
                            factCostProduction.originValue = 0;

                            factTotalCost.originValue = 0;
                            totalCost.originValue = 0;
                            me.reFillPromoForm(window, newModel, directorygrid);

                            me.updateStatusHistoryState();


                        }
                    });
                }
                else {
                    window.setLoading(false);
                    App.Notify.pushError(l10n.ns('tpm', 'text').value('failedLoadData'));
                }

            },
            error: function (data) {
                window.setLoading(false);
                App.Notify.pushError(data.responseJSON["odata.error"].innererror.message);
            }
        });
    },

    onActivityExportPromoProductsClick: function (button) {
        var me = this;
        var panel = button.up('promoeditorcustom');

        var actionName = 'SupportAdminExportXLSX';
        var resource = 'PromoProducts';

        var record = this.getRecord(panel);
        var promoId = record.get('Id');

        panel.setLoading(true);

        var parameters = {
            promoId: promoId
        };

        App.Util.makeRequestWithCallback(resource, actionName, parameters, function (data) {
            $.connection.sessionHub.server.startMoniringHandler(data.httpResponse.data.value)
                .done(function () {
                    panel.setLoading(false);
                })
                .fail(function (reason) {
                    console.log("SignalR connection failed: " + reason);
                    panel.setLoading(false);
                });

        }, function (data) {
            panel.setLoading(false);
            App.Notify.pushError(me.getErrorMessage(data));
        });
    },

    fillWorkFlowPanel: function (panel, result, approvalhistory, promoeditorcustom) {
        var tpl = Ext.create('App.view.tpm.common.approvalStatusStateTpl').formatTpl;
        var itemsArray = [];
        var promo = null;
        var onApprovalState = null;
        var promoStatusName = 'Draft';
        debugger;
        if (!panel.isDestroyed) {
            var panelWidthRatio = panel.getWidth() / 1160;
            //Small sizes correction:
            if (panelWidthRatio * 130 < 120) {
                panelWidthRatio = panelWidthRatio * 0.95;
            } else if (panelWidthRatio * 130 < 110) {
                panelWidthRatio = panelWidthRatio * 0.85;
            } else if (panelWidthRatio * 130 < 100) {
                panelWidthRatio = panelWidthRatio * 0.60;
            }
            var panelHeightRatio = (panel.getWidth() / 1160) * (100 / 130);

            if (!result.isEmpty) {
                if (result.data.length == 0) {
                    promo = promoeditorcustom.model ? promoeditorcustom.model.data : promoeditorcustom.assignedRecord ? promoeditorcustom.assignedRecord.data : null;
                } else {
                    promo = result.data[0].Promo;
                }

                if (promo) {
                    promoStatusName = promo.PromoStatus == undefined ? promo.PromoStatusSystemName : promo.PromoStatus.SystemName
                    if (promo.IsGrowthAcceleration === true || promo.IsInExchange === true) {
                        if ((promo.IsCMManagerApproved === false || promo.IsCMManagerApproved === null)
                            && (promo.IsDemandPlanningApproved === false || promo.IsDemandPlanningApproved === null)
                            && (promo.IsDemandFinanceApproved === false || promo.IsDemandFinanceApproved === null))
                            onApprovalState = 'CMManagerGA';
                        if (promo.IsCMManagerApproved === true
                            && (promo.IsDemandPlanningApproved === false || promo.IsDemandPlanningApproved === null)
                            && (promo.IsDemandFinanceApproved === false || promo.IsDemandFinanceApproved === null))
                            onApprovalState = 'DemandPlanningGA';
                        if (promo.IsCMManagerApproved === true
                            && promo.IsDemandPlanningApproved === true
                            && (promo.IsDemandFinanceApproved === false || promo.IsDemandFinanceApproved === null))
                            onApprovalState = 'DemandFinanceGA';
                        if (promo.IsCMManagerApproved === true
                            && promo.IsDemandPlanningApproved === true
                            && promo.IsDemandFinanceApproved === true)
                            onApprovalState = 'GAManagerGA';
                    }
                    else {
                        if (promo.IsCMManagerApproved == true
                            && (promo.IsDemandPlanningApproved == false || promo.IsDemandPlanningApproved == null)
                            && promo.IsDemandFinanceApproved == true)
                            onApprovalState = 'DemandPlanningNonego';
                        else if ((promo.IsCMManagerApproved == false || promo.IsCMManagerApproved == null)
                            && (promo.IsDemandPlanningApproved == false || promo.IsDemandPlanningApproved == null)
                            && (promo.IsDemandFinanceApproved == false || promo.IsDemandFinanceApproved == null))
                            onApprovalState = 'CMManager';
                        else if (promo.IsCMManagerApproved == true
                            && (promo.IsDemandPlanningApproved == false || promo.IsDemandPlanningApproved == null)
                            && (promo.IsDemandFinanceApproved == false || promo.IsDemandFinanceApproved == null))
                            onApprovalState = 'DemandPlanning';
                        else if (promo.IsCMManagerApproved == true && promo.IsDemandPlanningApproved == true
                            && (promo.IsDemandFinanceApproved == false || promo.IsDemandFinanceApproved == null)) {
                            if (promoStatusName.toLowerCase() == "onapproval") {
                                onApprovalState = "ErrorApproval";
                            } else {
                                onApprovalState = 'DemandFinance';
                            }
                        }
                    }
                }
            }

            var svgHeight = panel.body.getHeight() / panelHeightRatio;
            if (isNaN(svgHeight) || svgHeight === undefined || svgHeight === null || svgHeight === 0) {
                svgHeight = 2500;
            }
            var svgWidth = panel.body.getWidth() / panelWidthRatio;
            if (isNaN(svgWidth) || svgWidth === undefined || svgWidth === null || svgWidth === 0) {
                svgWidth = 4500;
            }
            if (promo) {
                var settings = {
                    svgHeight: svgHeight,
                    svgWidth: svgWidth,
                    currentWidthRatio: panelWidthRatio,
                    currentHeightRatio: panelHeightRatio,
                    currentHeight: panel.body.getHeight(),
                    status: promoStatusName,
                    onApprovalState: onApprovalState,
                    isNonego: result.isNoNegoPassed == undefined ? false : result.isNoNegoPassed,
                    statusHistory: result.data == undefined ? [] : result.data,
                    statusColors: result.statusColors
                }
            }
            else {
                var settings = {
                    svgHeight: svgHeight,
                    svgWidth: svgWidth,
                    currentWidthRatio: panelWidthRatio,
                    currentHeightRatio: panelHeightRatio,
                    currentHeight: panel.body.getHeight(),
                    status: 'draft',
                    onApprovalState: onApprovalState,
                    isNonego: result.isNoNegoPassed == undefined ? false : result.isNoNegoPassed,
                    statusHistory: result.data == undefined ? [] : result.data,
                    statusColors: result.statusColors
                }
            }

            itemsArray.push({
                html: tpl.apply(settings),
            });

            panel.removeAll();
            panel.add(itemsArray);
            panel.doLayout();

            var elementsWithTips = Ext.select('div[toolTip*=]');

            elementsWithTips.elements.forEach(function (el) {
                var me = el;
                me.fieldLabelTip = Ext.create('Ext.tip.ToolTip', {
                    target: me,
                    preventLoseFocus: true,
                    trackMouse: true,
                    html: me.getAttribute('tooltip'),
                    dismissDelay: 15000
                });
            })

            var workflowBtn = approvalhistory.down('button[id=workflowBtn]');
            var historyBtn = approvalhistory.down('button[id=historyBtn]');
            var dateOfChangeLable = approvalhistory.down('label');
            var reverseBtn = approvalhistory.down('button[id=reverseBtn]');

            if (historyBtn) {
                historyBtn.removeCls('selected');
            }
            workflowBtn.addClass('selected');
            dateOfChangeLable.hide();
            reverseBtn.hide();

            approvalhistory.isLoaded = false;
            approvalhistory.historyArray = result.data == undefined ? null : result.data;
            approvalhistory.isNonego = result.isNoNegoPassed == undefined ? false : result.isNoNegoPassed;
            approvalhistory.promoStatus = promoStatusName;
            approvalhistory.statusColors = result.statusColors;
            if (promoeditorcustom.down('#btn_changes').hasCls('selected'))
                promoeditorcustom.setLoading(false);
        }
    },

    getOnApprovalFilterDP: function () {
        var date = new Date();
        date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);

        date = Ext.Date.add(date, Ext.Date.DAY, 9 * 7);

        var filter = {
            operator: "and",
            rules: [
                {
                    property: "PromoStatusName", operation: "Equals", value: 'On Approval'
                },
                {
                    property: "DispatchesStart", operation: "LessThan", value: date
                },
                {
                    property: "IsCMManagerApproved", operation: "Equals", value: true
                },
                {
                    property: "IsGrowthAcceleration", operation: "Equals", value: false
                },
                {
                    property: "IsInExchange", operation: "Equals", value: false
                },
                {
                    operator: "or",
                    rules: [
                        {
                            property: "IsDemandPlanningApproved", operation: "Equals", value: null
                        },
                        {
                            property: "IsDemandPlanningApproved", operation: "Equals", value: false
                        }
                    ]
                },
            ]
        };

        return filter;
    },

    getOnApprovalFilterDF: function () {
        var date = new Date();
        date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);

        date = Ext.Date.add(date, Ext.Date.DAY, 9 * 7);

        var filter = {
            operator: "and",
            rules: [
                {
                    property: "PromoStatusName", operation: "Equals", value: 'On Approval'
                },
                {
                    property: "DispatchesStart", operation: "LessThan", value: date
                },
                {
                    property: "IsCMManagerApproved", operation: "Equals", value: true
                },
                {
                    property: "IsDemandPlanningApproved", operation: "Equals", value: true
                },
                {
                    property: "IsGrowthAcceleration", operation: "Equals", value: false
                },
                {
                    property: "IsInExchange", operation: "Equals", value: false
                },
                {
                    operator: "or",
                    rules: [
                        {
                            property: "IsDemandFinanceApproved", operation: "Equals", value: null
                        },
                        {
                            property: "IsDemandFinanceApproved", operation: "Equals", value: false
                        }
                    ]
                },
            ]
        };

        return filter;
    },

    getOnApprovalFilterCMM: function () {
        var date = new Date();
        date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);

        date = Ext.Date.add(date, Ext.Date.DAY, 9 * 7);

        var filter = {
            operator: "and",
            rules: [
                {
                    property: "PromoStatusName", operation: "Equals", value: 'On Approval'
                },
                {
                    property: "DispatchesStart", operation: "LessThan", value: date
                },
                {
                    property: "IsGrowthAcceleration", operation: "Equals", value: false
                },
                {
                    property: "IsInExchange", operation: "Equals", value: false
                },
                {
                    operator: "or",
                    rules: [
                        {
                            property: "IsCMManagerApproved", operation: "Equals", value: null
                        },
                        {
                            property: "IsCMManagerApproved", operation: "Equals", value: false
                        }]

                }
            ]
        };

        return filter;
    },

    getOnApprovalGAFilterCMM: function () {
        var date = new Date();
        date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);

        date = Ext.Date.add(date, Ext.Date.DAY, 9 * 7);

        var filter = {
            operator: "and",
            rules: [
                {
                    property: "PromoStatusName", operation: "Equals", value: 'On Approval'
                },
                {
                    property: "DispatchesStart", operation: "LessThan", value: date
                },
                {
                    operator: "or",
                    rules: [
                        {
                            property: "IsGrowthAcceleration", operation: "Equals", value: true
                        },
                        {
                            property: "IsInExchange", operation: "Equals", value: true
                        },
                    ]
                },
                {
                    operator: "or",
                    rules: [
                        {
                            property: "IsCMManagerApproved", operation: "Equals", value: null
                        },
                        {
                            property: "IsCMManagerApproved", operation: "Equals", value: false
                        }
                    ]
                }
            ]
        };

        return filter;
    },

    getOnApprovalGAFilterDP: function () {
        var date = new Date();
        date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);

        date = Ext.Date.add(date, Ext.Date.DAY, 9 * 7);

        var filter = {
            operator: "and",
            rules: [
                {
                    property: "PromoStatusName", operation: "Equals", value: 'On Approval'
                },
                {
                    property: "DispatchesStart", operation: "LessThan", value: date
                },
                {
                    property: "IsCMManagerApproved", operation: "Equals", value: true
                },
                {
                    operator: "or",
                    rules: [
                        {
                            property: "IsGrowthAcceleration", operation: "Equals", value: true
                        },
                        {
                            property: "IsInExchange", operation: "Equals", value: true
                        },
                    ]
                },
                {
                    operator: "or",
                    rules: [
                        {
                            property: "IsDemandPlanningApproved", operation: "Equals", value: null
                        },
                        {
                            property: "IsDemandPlanningApproved", operation: "Equals", value: false
                        }
                    ]
                },
            ]
        };

        return filter;
    },

    getOnApprovalGAFilterDF: function () {
        var date = new Date();
        date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);

        date = Ext.Date.add(date, Ext.Date.DAY, 9 * 7);

        var filter = {
            operator: "and",
            rules: [
                {
                    property: "PromoStatusName", operation: "Equals", value: 'On Approval'
                },
                {
                    property: "DispatchesStart", operation: "LessThan", value: date
                },
                {
                    property: "IsCMManagerApproved", operation: "Equals", value: true
                },
                {
                    property: "IsDemandPlanningApproved", operation: "Equals", value: true
                },
                {
                    operator: "or",
                    rules: [
                        {
                            property: "IsGrowthAcceleration", operation: "Equals", value: true
                        },
                        {
                            property: "IsInExchange", operation: "Equals", value: true
                        },
                    ]
                },
                {
                    operator: "or",
                    rules: [
                        {
                            property: "IsDemandFinanceApproved", operation: "Equals", value: null
                        },
                        {
                            property: "IsDemandFinanceApproved", operation: "Equals", value: false
                        }
                    ]
                },
            ]
        };

        return filter;
    },

    getOnApprovalGAFilterGAM: function () {
        var date = new Date();
        date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);

        date = Ext.Date.add(date, Ext.Date.DAY, 9 * 7);

        var filter = {
            operator: "and",
            rules: [
                {
                    property: "PromoStatusName", operation: "Equals", value: 'On Approval'
                },
                {
                    property: "DispatchesStart", operation: "LessThan", value: date
                },
                {
                    property: "IsCMManagerApproved", operation: "Equals", value: true
                },
                {
                    property: "IsDemandPlanningApproved", operation: "Equals", value: true
                },
                {
                    property: "IsDemandFinanceApproved", operation: "Equals", value: true
                },
                {
                    operator: "or",
                    rules: [
                        {
                            property: "IsGrowthAcceleration", operation: "Equals", value: true
                        },
                        {
                            property: "IsInExchange", operation: "Equals", value: true
                        },
                    ]
                },
                {
                    operator: "or",
                    rules: [
                        {
                            property: "IsGAManagerApproved", operation: "Equals", value: null
                        },
                        {
                            property: "IsGAManagerApproved", operation: "Equals", value: false
                        }
                    ]
                }
            ]
        };

        return filter;
    },
});