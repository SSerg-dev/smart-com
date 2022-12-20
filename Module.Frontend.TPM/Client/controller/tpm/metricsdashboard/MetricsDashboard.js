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
                'metricsdashboard #clickPanel': {
                    afterrender: this.onClientPanelAfterRender
                },


            }
        });
    },

    onRender: function (window) {
        //this.getCard(window);
    },

    onClientPanelAfterRender: function (panel) {
        var metricsDashboardController = this;
        panel.body.addListener('click', function () {
            metricsDashboardController.onClickPanelClick(panel);
        });
    },
    onClickPanelClick: function (panel) {
        var metricsDashboardController = App.app.getController('tpm.metricsdashboard.MetricsDashboard');
        var metricsDashboard = panel.up('metricsdashboard');
        var metricsDashboardClientPeriodChooseWindow = Ext.widget('metricsdashboardclientperiodchoosewindow');
        var clientTreeField = metricsDashboardController.getClientTreeField(metricsDashboardClientPeriodChooseWindow);
        var periodField = metricsDashboardClientPeriodChooseWindow.down('#PeriodField');

        var clientTreeRecord = metricsDashboard['choosenClientTreeRecord'];
        var periodRecord = metricsDashboard['choosenPeriod'];
        if (clientTreeRecord) {
            clientTreeField.setValue(new App.model.tpm.clienttree.ClientTree({
                Id: clientTreeRecord.data.Id,
                Name: clientTreeRecord.data.Name,
                ObjectId: clientTreeRecord.data.ObjectId,
                IsOnInvoice: clientTreeRecord.data.IsOnInvoice,
            }));
        }

        metricsDashboardClientPeriodChooseWindow.down('#choose').addListener('click', metricsDashboardController.onMetricsDashboardClientPeriodChooseButtonClick);
        metricsDashboardClientPeriodChooseWindow.show();

        periodField.setValue(periodRecord);
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

        panel.up('metricsdashboard')['choosenClientTreeRecord'] = new App.model.tpm.clienttree.ClientTree();
        panel.up('metricsdashboard')['choosenPeriod'] = null;

        clientTreeStoreProxy.extraParams.needBaseClients = false;
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
        var metricsDashboardController = App.app.getController('tpm.metricsdashboard.MetricsDashboard');;
        var metricsDashboardClientYearWindowChoose = button.up('metricsdashboardclientperiodchoosewindow');
        var metricsDashboard = Ext.ComponentQuery.query('metricsdashboard')[0];
        var clientTreeField = metricsDashboardClientYearWindowChoose.down('#ClientTreeField');
        var periodField = metricsDashboardClientYearWindowChoose.down('#PeriodField');
        var panel1 = Ext.ComponentQuery.query('container #panel1')[0];
        var panel2 = Ext.ComponentQuery.query('container #panel2')[0];
        var period = Ext.ComponentQuery.query('#PeriodMetricsId')[0];
        var client = Ext.ComponentQuery.query('#ClientMetricsId')[0];
        clientTreeField.validate();
        periodField.validate();

        var selectedClientRecord = clientTreeField.getRecord();
        var selectedPeriod = periodField.getValue();

        if (clientTreeField.isValid() && periodField.isValid()) {
            metricsDashboard['choosenClientTreeRecord'] = selectedClientRecord;
            metricsDashboard['choosenPeriod'] = selectedPeriod;
            panel1.items.clear();
            panel2.items.clear();
            metricsDashboardController.getCard(panel1);
            period.setText(periodField.getRawValue());
            client.setText(clientTreeField.getValue());
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
            MenuMgr.setCurrentMenu(MenuMgr.getCurrentMenu().getParent()); // очистеть меню
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
    getPPAperiod: function () {
        var dateStart = new Date();
        dateStart.setHours(dateStart.getHours() + (dateStart.getTimezoneOffset() / 60) + 3);

        var dateEnd = Ext.Date.add(dateStart, Ext.Date.DAY, 7 * 8);
        var filter = null;
        var widget = 'promo';
        var text = "Actions";
        var panel = 'panel3';
        var image = 'adjust_data.png';
        var color = '#00009b';
        var buttonColor = '#eef2fc';
        return { filter: filter, widget: widget, text: text, panel: panel, image: image, color: color, buttonColor: buttonColor };
    },
    getPCTperiod: function () {
        var dateStart = new Date();
        dateStart.setHours(dateStart.getHours() + (dateStart.getTimezoneOffset() / 60) + 3);

        dateStart = new Date(dateStart.getFullYear(), 0, 1);
        var filter = null;
        var widget = 'promo';
        var text = "Actions";
        var panel = 'panel3';
        var image = 'adjust_data.png';
        var color = '#0e0d9e';
        var buttonColor = '#eef2fc';
        return { filter: filter, widget: widget, text: text, panel: panel, image: image, color: color, buttonColor: buttonColor };
    },
    getPSFA: function () {
        var dateStart = new Date();
        dateStart.setHours(dateStart.getHours() + (dateStart.getTimezoneOffset() / 60) + 3);

        dateStart = new Date(dateStart.getFullYear(), 0, 1);
        var filter = null;
        var widget = 'promo';
        var text = "Actions";
        var panel = 'panel3';
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

        var clientTreeRecord = view['choosenClientTreeRecord'];
        var periodRecord = view['choosenPeriod'];

        var parameters = {
            userrole: userrole, clientTreeId: clientTreeRecord.get('Id'), period: periodRecord.ticks
        };
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

                if (result.PPA >= 95) {
                    button.down('#glyphRight').style = 'background-color:' + '#66BB6A';
                } else if (result.PPA >= 90) {
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
                if (view.query('#' + buttons.panel)[0])
                    view.query('#' + buttons.panel)[0].add(button);
                Ext.get(button.down('#buttonArrow').id + '-btnIconEl').setStyle('color', buttons.color);

                //PCT
                buttons = me.getPCT();
                var button = Ext.widget('metricsdashboadpanel');
                button.widget = buttons.widget;
                button.filter = buttons.filter;

                button.down('#NameLabel').setText('PCT');
                button.down('#CountLabel').setText(result.PCT + '%');
                button.down('#CountLabel_LSV').setText('LSV: ' + result.PCT_LSV);

                if (result.PCT >= 90) {
                    button.down('#glyphRight').style = 'background-color:' + '#66BB6A';
                } else if (result.PCT >= 85) {
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
                if (view.query('#' + buttons.panel)[0])
                    view.query('#' + buttons.panel)[0].add(button);
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
                if (view.query('#' + buttons.panel)[0])
                    view.query('#' + buttons.panel)[0].add(button);
                Ext.get(button.down('#buttonArrow').id + '-btnIconEl').setStyle('color', buttons.color);

                //PPA period
                //buttons = me.getPPAperiod();
                //var button = Ext.widget('metricsdashboadpanel');
                //button.widget = buttons.widget;
                //button.filter = buttons.filter;

                //button.down('#NameLabel').setText('PPA');
                //button.down('#CountLabel').setText(result.PPA + '%');
                //button.down('#CountLabel_LSV').setText('LSV: ' + result.PPA_LSV);

                //if (result.PAD >= 95) {
                //    button.down('#glyphRight').style = 'background-color:' + '#66BB6A';
                //} else if (result.PAD >= 90) {
                //    button.down('#glyphRight').style = 'background-color:' + '#FFB74D';
                //} else {
                //    button.down('#glyphRight').style = 'background-color:' + 'red';
                //}

                //button.down('button').style = 'background-color:' + buttons.buttonColor;
                //button.down('#buttonPanel').style = 'background-color:' + buttons.buttonColor;
                //button.down('#buttonArrow').style = 'background-color:' + buttons.buttonColor;
                //button.down('#glyphRight').setSrc('/Bundles/style/images/' + buttons.image);
                //if (buttons.style) {
                //    button.down('#CountLabel').addCls('panel-time-critical-standart');
                //}

                //button.down('button').setText(buttons.text);
                //if (view.query('#' + buttons.panel)[0])
                //    view.query('#' + buttons.panel)[0].add(button);
                //button.down('#buttonText').setDisabled(true);
                //button.down('#buttonArrow').setDisabled(true);

                //PCT period
                //buttons = me.getPCTperiod();
                //var button = Ext.widget('metricsdashboadpanel');
                //button.widget = buttons.widget;
                //button.filter = buttons.filter;

                //button.down('#NameLabel').setText('PCT');
                //button.down('#CountLabel').setText(result.PCT + '%');
                //button.down('#CountLabel_LSV').setText('LSV: ' + result.PCT_LSV);

                //if (result.PAD >= 90) {
                //    button.down('#glyphRight').style = 'background-color:' + '#66BB6A';
                //} else if (result.PAD >= 85) {
                //    button.down('#glyphRight').style = 'background-color:' + '#FFB74D';
                //} else {
                //    button.down('#glyphRight').style = 'background-color:' + 'red';
                //}

                //button.down('button').style = 'background-color:' + buttons.buttonColor;
                //button.down('#buttonPanel').style = 'background-color:' + buttons.buttonColor;
                //button.down('#buttonArrow').style = 'background-color:' + buttons.buttonColor;
                //button.down('#glyphRight').setSrc('/Bundles/style/images/' + buttons.image);
                //if (buttons.style) {
                //    button.down('#CountLabel').addCls('panel-time-critical-standart');
                //}

                //button.down('button').setText(buttons.text);
                //if (view.query('#' + buttons.panel)[0])
                //    view.query('#' + buttons.panel)[0].add(button);
                //button.down('#buttonText').setDisabled(true);
                //button.down('#buttonArrow').setDisabled(true);

                //PCT period
                buttons = me.getPSFA();
                var button = Ext.widget('metricsdashboadpanel');
                button.widget = buttons.widget;
                button.filter = buttons.filter;

                button.down('#NameLabel').setText('P-SFA');
                button.down('#CountLabel').setText(result.PCT + '%');
                button.down('#CountLabel_LSV').setText('LSV: ' + result.PCT_LSV);

                if (result.PSFA >= 80) {
                    button.down('#glyphRight').style = 'background-color:' + '#66BB6A';
                } else if (result.PSFA >= 75) {
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
                if (view.query('#' + buttons.panel)[0])
                    view.query('#' + buttons.panel)[0].add(button);
                button.down('#buttonText').setDisabled(true);
                button.down('#buttonArrow').setDisabled(true);

                mask.hide();
            }
        }, function (data) {
            mask.hide();
            console.log(data);
        });
    }
});
