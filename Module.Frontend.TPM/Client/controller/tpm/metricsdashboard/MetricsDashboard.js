Ext.define('App.controller.tpm.metricsdashboard.MetricsDashboard', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'metricsdashboard #panel1': {
                    beforerender: this.onMetricsDashboardBeforeRender,
                    afterrender: this.onRender
                },
                'metricsdashboard button[itemId!=ok]': {
                    click: this.onButtonClick2
                },



            }
        });
    },

    onRender: function (window) {
        this.getCard(window);
    },

    onMetricsDashboardBeforeRender: function (panel) {
        
        panel.setLoading(true);
        var me = this;
        var metricsDashboardController = me;
        var metricsDashboardClientPeriodChoose = Ext.widget('metricsdashboardclientperiodchoosewindow');
        var clientTreeField = metricsDashboardController.getClientTreeField(metricsDashboardClientPeriodChoose);

        metricsDashboardClientPeriodChoose.down('#choose').addListener('click', metricsDashboardController.onMetricsDashboardClientPeriodChooseButtonClick);

        var clientTreeStore = metricsDashboardController.getClientTreeStore();
        var clientTreeStoreProxy = clientTreeStore.getProxy();
        
        panel['choosenClientTreeRecord'] = new App.model.tpm.clienttree.ClientTree();
        panel['choosenYear'] = null;

        clientTreeStoreProxy.extraParams.needBaseClients = true;
        clientTreeStoreProxy.extraParams['node'] = 'root';

        // Получаем базовых клиентов для текущего пользователя.
        // Если только один базовый, то выбираем его по умолчанию.
        metricsDashboardClientPeriodChoose.setLoading(true);
        clientTreeStore.on({
            load: {
                fn: function (store, records) {
                    panel['loaderState'] = true;
                    var baseClients = [];

                    var stack = records;
                    while (stack.length > 0) {
                        var currentNode = stack.pop();
                        if (currentNode) {
                            if (currentNode.IsBaseClient) {
                                baseClients.push(currentNode);
                            }

                            var childs = [];
                            if ((currentNode.raw && currentNode.raw.children)) {
                                childs = Array.isArray(currentNode.raw.children) ? currentNode.raw.children : [currentNode.raw.children];
                            }
                            else if (currentNode.children) {
                                childs = currentNode.children;
                            }

                            childs.forEach(function (x) {
                                stack.push(x);
                            })
                        }
                    }

                    if (baseClients.length == 1) {
                        clientTreeField.setValue(new App.model.tpm.clienttree.ClientTree({
                            Id: baseClients[0].Id,
                            Name: baseClients[0].Name,
                            ObjectId: baseClients[0].ObjectId,
                            IsOnInvoice: baseClients[0].IsOnInvoice,
                        }));

                        metricsDashboardClientPeriodChoose.down('#choose').fireEvent('click', metricsDashboardClientPeriodChoose.down('#choose'));
                    }
                    else {
                        metricsDashboardClientPeriodChoose.show(null, function () {
                            panel.setLoading(false);
                        });
                    }

                    clientTreeStoreProxy.extraParams.needBaseClients = false;
                    delete clientTreeStoreProxy.extraParams.node;
                },
                single: true
            }
        });

        clientTreeStore.load();
    },

    getClientTreeField: function (parent) {
        var clientTreeField = null;
        if (parent) {
            clientTreeField = parent.down('#ClientTreeField');
        }
        else {
            clientTreeField = Ext.ComponentQuery.query('#ClientTreeField')[0];
        }
        return clientTreeField;
    },

    getClientTreeStore: function () {
        var clientTreeStore = Ext.create('Ext.data.Store', {
            model: 'App.model.tpm.clienttree.ClientTree',
        });

        return clientTreeStore;
    },

    onMetricsDashboardClientPeriodChooseButtonClick: function (button) {
        var me = this;
        var metricsDashboardController = me;
        var clientDashboard = Ext.ComponentQuery.query('clientdashboard')[0];
        var metricsDashboardClientYearWindowChoose = button.up('metricsdashboardclientperiodchoosewindow');
        var clientTreeField = metricsDashboardClientYearWindowChoose.down('#ClientTreeField');
        var yearField = metricsDashboardClientYearWindowChoose.down('#YearField');

        clientTreeField.validate();
        yearField.validate();

        var selectedClientRecord = clientTreeField.getRecord();
        var selectedYear = yearField.getValue();

        if (clientTreeField.isValid() && yearField.isValid()) {
            clientDashboard.show();
            var accountInformationButton = clientDashboard.down('#accountInformationButton');
            var promoWeeksButton = clientDashboard.down('#promoWeeksButton');
            var accountInformation = clientDashboard.down('accountinformation');
            var accountInformationRS = clientDashboard.down('accountinformationrs');

            clientDashboard['choosenClientTreeRecord'] = selectedClientRecord;
            clientDashboard['choosenYear'] = selectedYear;

            if (selectedClientRecord.data.IsOnInvoice != null) {
                if (selectedClientRecord.data.IsOnInvoice) {
                    accountInformation.down('#accountInformationClientType').setText('On Invoice');
                    accountInformationRS.down('#accountInformationClientTypeRS').setText('On Invoice');
                } else {
                    accountInformation.down('#accountInformationClientType').setText('Off Invoice');
                    accountInformationRS.down('#accountInformationClientTypeRS').setText('Off Invoice');
                }
            } else {
                accountInformation.down('#accountInformationClientType').setText('');
                accountInformationRS.down('#accountInformationClientTypeRS').setText('');
            }

            accountInformation.down('#accountInformationClientText').setText(selectedClientRecord.data.Name);
            accountInformation.down('#accountInformationYearText').setText(selectedYear);

            accountInformationRS.down('#accountInformationClientTextRS').setText(selectedClientRecord.data.Name);
            accountInformationRS.down('#accountInformationYearTextRS').setText(selectedYear);

            metricsDashboardController.loadStoreWithFilters(clientDashboard, metricsDashboardController.fillAccountInformationCallback, metricsDashboardController.fillPromoWeeksCallback, false);
            button.up('window').close();
        }
    },
    onButtonClick2: function (window) {
        var panel = window.up().up();
        var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
        var promoController = App.app.getController('core.Main');
        var vc = promoController.getViewContainer(),
            view = vc.getComponent(panel.widget);

        if (!view) {
            view = Ext.widget(panel.widget);

            if (view.isXType('associateddirectoryview')) {
                vc.addCls('associated-directory');
            } else {
                vc.removeCls('associated-directory');
            }

            Ext.suspendLayouts();
            vc.removeAll();
            vc.add(view);
            Ext.resumeLayouts(true);

            var grid = view.down('directorygrid');
            var store = grid.getStore();
            store.setFixedFilter('hiddenExtendedFilter', panel.filter);
            vc.doLayout();
        }
    },
    //Promo to close
    getFinishedFilter: function () {
        var filter = {
            operator: "and",
            rules: [
                {
                    property: "PromoStatusName", operation: "Equals", value: 'Finished'
                },
                {
                    operator: "or",
                    rules: [{
                        operator: "and",
                        rules: [
                            {
                                property: "InOut", operation: "Equals", value: false
                            }, {
                                property: "ActualPromoUpliftPercent", operation: "NotEqual", value: 0
                            }, {
                                property: "ActualPromoUpliftPercent", operation: "NotEqual", value: null
                            }, {
                                property: "ActualPromoBaselineLSV", operation: "NotEqual", value: 0
                            }, {
                                property: "ActualPromoBaselineLSV", operation: "NotEqual", value: null
                            }, {
                                property: "ActualPromoIncrementalLSV", operation: "NotEqual", value: 0
                            }, {
                                property: "ActualPromoIncrementalLSV", operation: "NotEqual", value: null
                            }, {
                                property: "ActualPromoLSV", operation: "NotEqual", value: 0
                            }, {
                                property: "ActualPromoLSV", operation: "NotEqual", value: null
                            }, {
                                property: "ActualPromoLSVByCompensation", operation: "NotEqual", value: 0
                            }, {
                                property: "ActualPromoLSVByCompensation", operation: "NotEqual", value: null
                            }
                        ]
                    }, {
                        operator: "and",
                        rules: [
                            {
                                property: "InOut", operation: "Equals", value: true
                            }, {
                                property: "ActualPromoIncrementalLSV", operation: "NotEqual", value: 0
                            }, {
                                property: "ActualPromoIncrementalLSV", operation: "NotEqual", value: null
                            }, {
                                property: "ActualPromoLSV", operation: "NotEqual", value: 0
                            }, {
                                property: "ActualPromoLSV", operation: "NotEqual", value: null
                            }, {
                                property: "ActualPromoLSVByCompensation", operation: "NotEqual", value: 0
                            }, {
                                property: "ActualPromoLSVByCompensation", operation: "NotEqual", value: null
                            }
                        ]
                    }],
                },
            ]
        };

        return filter;
    },


    getPPA: function () {
        var dateStart = new Date();
        dateStart.setHours(dateStart.getHours() + (dateStart.getTimezoneOffset() / 60) + 3);

        var dateEnd = Ext.Date.add(dateStart, Ext.Date.DAY, 7 * 8);
        var filter = {
            operator: "and",
            rules: [
                {
                    property: "DispatchesStart", operation: "LessOrEqual", value: dateEnd
                },
                {
                    property: "DispatchesStart", operation: "GreaterOrEqual", value: dateStart
                },
                {
                    property: "PromoStatusName", operation: "In", value: ['Draft(published)', 'On Approval']
                }
                //    , {
                //    operator: "or",
                //    rules: [{
                //        property: "PromoStatusName", operation: "Equals", value: 'On Approval'
                //    },
                //    {
                //        property: "PromoStatusName", operation: "Equals", value: 'Draft(published)'
                //    }]
                //}
            ]
        };
        var widget = 'promo';
        var text = "Actions";
        var panel = 'panel1';
        var image = 'adjust_data.png';
        var color = '#00009b';
        var buttonColor = '#eef2fc';
        return { filter: filter, widget: widget, text: text, panel: panel, image: image, color: color, buttonColor: buttonColor };
    },
    getPCT: function () {
        var dateStart = new Date();
        dateStart.setHours(dateStart.getHours() + (dateStart.getTimezoneOffset() / 60) + 3);

        var dateEnd = Ext.Date.add(dateStart, Ext.Date.DAY, - 7 * 7);
        dateStart = new Date(dateStart.getFullYear(), 0, 1);
        var filter = {
            operator: "and",
            rules: [
                {
                    property: "DispatchesStart", operation: "LessOrEqual", value: dateEnd
                }, {
                    property: "DispatchesStart", operation: "GreaterOrEqual", value: dateStart
                },
                this.getFinishedFilter()
            ]
        };
        var widget = 'promo';
        var text = "Actions";
        var panel = 'panel1';
        var image = 'adjust_data.png';
        var color = '#0e0d9e';
        var buttonColor = '#eef2fc';
        return { filter: filter, widget: widget, text: text, panel: panel, image: image, color: color, buttonColor: buttonColor };
    },
    getPAD: function () {
        var dateStart = new Date();
        dateStart.setHours(dateStart.getHours() + (dateStart.getTimezoneOffset() / 60) + 3);

        var dateEnd = Ext.Date.add(dateStart, Ext.Date.DAY, - 7 * 7);
        dateStart = new Date(dateStart.getFullYear(), 0, 1);
        var filter = {
            operator: "and",
            rules: [
                {
                    property: "DispatchesStart", operation: "LessOrEqual", value: dateEnd
                }, {
                    property: "DispatchesStart", operation: "GreaterOrEqual", value: dateStart
                },
                this.getFinishedFilter()
            ]
        };
        var widget = 'promo';
        var text = "Actions";
        var panel = 'panel2';
        var image = 'adjust_data.png';
        var color = '#0e0d9e';
        var buttonColor = '#eef2fc';
        return { filter: filter, widget: widget, text: text, panel: panel, image: image, color: color, buttonColor: buttonColor };
    },
    getCard: function (window) {
        var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
        var view = window.up('metricsdashboard');
        var mask = new Ext.LoadMask(view, { msg: "Please wait..." });


        mask.show();
        var me = this;
        var userrole = breeze.DataType.String.fmtOData(currentRole);

        var parameters = { userrole: userrole };

        App.Util.makeRequestWithCallback('Promoes', 'GetLiveMetricsDashboard', parameters, function (data) {
            if (data) {
                var result = Ext.JSON.decode(data.httpResponse.data.value);
                var buttons;

                //PPA
                buttons = me.getPPA();
                var button = Ext.widget('metricsdashboadpanel');
                button.widget = buttons.widget;
                button.filter = buttons.filter;

                button.down('#NameLabel').setText('PPA');
                button.down('#CountLabel').setText(result.PPA + '%');
                button.down('#CountLabel_LSV').setText('LSV: ' + result.PPA_LSV);

                if (result.PAD >= 95) {
                    button.down('#glyphRight').style = 'background-color:' + '#66BB6A';
                } else if (result.PAD >= 90) {
                    button.down('#glyphRight').style = 'background-color:' + '#FFB74D';
                } else {
                    button.down('#glyphRight').style = 'background-color:' + 'red';
                }

                button.down('button').style = 'background-color:' + buttons.buttonColor;
                button.down('#buttonPanel').style = 'background-color:' + buttons.buttonColor;
                button.down('#buttonArrow').style = 'background-color:' + buttons.buttonColor;
                button.down('#glyphRight').setSrc('/Bundles/style/images/' + buttons.image);
                if (buttons.style) {
                    button.down('#CountLabel').addCls('panel-time-critical-standart');
                }

                button.down('button').setText(buttons.text);
                if (view.down('#' + buttons.panel))
                    view.down('#' + buttons.panel).add(button);
                Ext.get(button.down('#buttonArrow').id + '-btnIconEl').setStyle('color', buttons.color);

                //PCT
                buttons = me.getPCT();
                var button = Ext.widget('metricsdashboadpanel');
                button.widget = buttons.widget;
                button.filter = buttons.filter;

                button.down('#NameLabel').setText('PCT');
                button.down('#CountLabel').setText(result.PCT + '%');
                button.down('#CountLabel_LSV').setText('LSV: ' + result.PCT_LSV);

                if (result.PAD >= 90) {
                    button.down('#glyphRight').style = 'background-color:' + '#66BB6A';
                } else if (result.PAD >= 85) {
                    button.down('#glyphRight').style = 'background-color:' + '#FFB74D';
                } else {
                    button.down('#glyphRight').style = 'background-color:' + 'red';
                }

                button.down('button').style = 'background-color:' + buttons.buttonColor;
                button.down('#buttonPanel').style = 'background-color:' + buttons.buttonColor;
                button.down('#buttonArrow').style = 'background-color:' + buttons.buttonColor;
                button.down('#glyphRight').setSrc('/Bundles/style/images/' + buttons.image);
                if (buttons.style) {
                    button.down('#CountLabel').addCls('panel-time-critical-standart');
                }

                button.down('button').setText(buttons.text);
                if (view.down('#' + buttons.panel))
                    view.down('#' + buttons.panel).add(button);
                Ext.get(button.down('#buttonArrow').id + '-btnIconEl').setStyle('color', buttons.color);

                //PAD
                buttons = me.getPAD();
                var button = Ext.widget('metricsdashboadpanel');
                button.widget = buttons.widget;
                button.filter = buttons.filter;

                button.down('#NameLabel').setText('PAD');
                button.down('#CountLabel').setText(result.PAD + '%');
                button.down('#CountLabel_LSV').setText('LSV: ' + result.PAD_LSV);

                if (result.PAD == 100) {
                    button.down('#glyphRight').style = 'background-color:' + '#66BB6A';
                } else if (result.PAD >= 95) {
                    button.down('#glyphRight').style = 'background-color:' + '#FFB74D';
                } else {
                    button.down('#glyphRight').style = 'background-color:' + 'red';
                }

                button.down('button').style = 'background-color:' + buttons.buttonColor;
                button.down('#buttonPanel').style = 'background-color:' + buttons.buttonColor;
                button.down('#buttonArrow').style = 'background-color:' + buttons.buttonColor;
                button.down('#glyphRight').setSrc('/Bundles/style/images/' + buttons.image);
                if (buttons.style) {
                    button.down('#CountLabel').addCls('panel-time-critical-standart');
                }

                button.down('button').setText(buttons.text);
                if (view.down('#' + buttons.panel))
                    view.down('#' + buttons.panel).add(button);
                Ext.get(button.down('#buttonArrow').id + '-btnIconEl').setStyle('color', buttons.color);

                mask.hide();
            }
        }, function (data) {
            mask.hide();
            console.log(data);
        });
    }
});
