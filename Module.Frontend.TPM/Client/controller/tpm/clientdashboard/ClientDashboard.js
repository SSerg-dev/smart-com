Ext.define('App.controller.tpm.clientdashboard.ClientDashboard', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'clientdashboard': {
                    beforerender: this.onClientDashboardBeforeRender,
                    afterrender: this.onClientDashboardAfterRender,
                },

                'clientdashboard #refresh': {
                    click: this.onRefreshButtonClick
                },

                'accountinformation #panelButton': {
                    afterrender: this.onClientTreePanelAfterRender
                },
                'accountinformation #detailsButton': {
                    click: this.onDetailButtonClick
                }, 
            }
        });
    },
    onDetailButtonClick: function (button) { 
        var window = Ext.widget('clentdashboarddetailswindow');
        var controller = App.app.getController('tpm.clientdashboard.ClientDashboard');
        window.show();
        window.down('#PromoTiCostPlanPercent').originalValue = button.PromoTiCostPlanPercent.toFixed(2);
        window.down('#PromoTiCostPlan').originalValue = button.PromoTiCostPlan.toFixed(2);
        window.down('#PromoTiCostYTD').originalValue = button.PromoTiCostYTD.toFixed(2);
        window.down('#PromoTiCostYTDPercent').originalValue = button.PromoTiCostYTDPercent.toFixed(2);
        window.down('#PromoTiCostYEE').originalValue = button.PromoTiCostYEE.toFixed(2);
        window.down('#PromoTiCostYEEPercent').originalValue = button.PromoTiCostYEEPercent.toFixed(2);

        window.down('#NonPromoTiCostPlanPercent').originalValue = button.NonPromoTiCostPlanPercent.toFixed(2);
        window.down('#NonPromoTiCostPlan').originalValue = button.NonPromoTiCostPlan.toFixed(2);
        window.down('#NonPromoTiCostYTD').originalValue = button.NonPromoTiCostYTD.toFixed(2);
        window.down('#NonPromoTiCostYTDPercent').originalValue = button.NonPromoTiCostYTDPercent.toFixed(2);
        window.down('#NonPromoTiCostYEE').originalValue = button.NonPromoTiCostYEE.toFixed(2);
        window.down('#NonPromoTiCostYEEPercent').originalValue = button.NonPromoTiCostYEEPercent.toFixed(2);

        controller.setValueFieldColor(window.down('#PromoTiCostYTDPercent'), button.PromoTiCostPlanPercent, button.PromoTiCostYTDPercent, window.down('#PromoTiCostYTDPercentArrow'));
        controller.setValueFieldColor(window.down('#PromoTiCostYEEPercent'), button.PromoTiCostPlanPercent, button.PromoTiCostYEEPercent, window.down('#PromoTiCostYEEPercentArrow'));
        controller.setValueFieldColor(window.down('#PromoTiCostYTD'), button.PromoTiCostPlan, button.PromoTiCostYTD, window.down('#PromoTiCostYTDArrow'));
        controller.setValueFieldColor(window.down('#PromoTiCostYEE'), button.PromoTiCostPlan, button.PromoTiCostYEE, window.down('#PromoTiCostYEEArrow'));

        controller.setValueFieldColor(window.down('#NonPromoTiCostYTDPercent'), button.NonPromoTiCostPlanPercent, button.NonPromoTiCostYTDPercent, window.down('#NonPromoTiCostYTDPercentArrow'));
        controller.setValueFieldColor(window.down('#NonPromoTiCostYEEPercent'), button.NonPromoTiCostPlanPercent, button.NonPromoTiCostYEEPercent, window.down('#NonPromoTiCostYEEPercentArrow'));
        controller.setValueFieldColor(window.down('#NonPromoTiCostYTD'), button.NonPromoTiCostPlan, button.NonPromoTiCostYTD, window.down('#NonPromoTiCostYTDArrow'));
        controller.setValueFieldColor(window.down('#NonPromoTiCostYEE'), button.NonPromoTiCostPlan, button.NonPromoTiCostYEE, window.down('#NonPromoTiCostYEEArrow'));
        controller.setupValuesAndTips()
    },
   
    onClientDashboardBeforeRender: function (panel) {
        panel.setLoading(true);

        var clientDashboardController = App.app.getController('tpm.clientdashboard.ClientDashboard'); 
        var clientDashboardClientYearWindowChoose = Ext.widget('clientdashboardclientyearchoosewindow');
        var clientTreeField = clientDashboardController.getClientTreeField(clientDashboardClientYearWindowChoose);

        clientDashboardClientYearWindowChoose.down('#choose').addListener('click', clientDashboardController.onClientDashboardClientYearWindowChooseButtonClick);

        var clientTreeStore = clientDashboardController.getClientTreeStore();
        var clientTreeStoreProxy = clientTreeStore.getProxy();

        panel['choosenClientTreeRecord'] = new App.model.tpm.clienttree.ClientTree();
        panel['choosenYear'] = null;

        clientTreeStoreProxy.extraParams.needBaseClients = true;
        clientTreeStoreProxy.extraParams['node'] = 'root';

        // Получаем базовых клиентов для текущего пользователя.
        // Если только один базовый, то выбираем его по умолчанию.
        clientDashboardClientYearWindowChoose.setLoading(true);
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

                        clientDashboardClientYearWindowChoose.down('#choose').fireEvent('click', clientDashboardClientYearWindowChoose.down('#choose'));
                    }
                    else {
                        clientDashboardClientYearWindowChoose.show(null, function () {
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

    onClientDashboardAfterRender: function (panel) {
        if (!panel['loaderState']) {
            panel.setLoading(true);
        }
    },

    onRefreshButtonClick: function (button) {
        var clientDashboardController = App.app.getController('tpm.clientdashboard.ClientDashboard'); 
        var clientDashboard = Ext.ComponentQuery.query('clientdashboard')[0];

        clientDashboardController.loadStoreWithFilters(clientDashboard, clientDashboardController.fillAccountInformationCallback, clientDashboardController.fillPromoWeeksCallback,true);
    },

    onClientDashboardClientYearWindowChooseButtonClick: function (button) {
        var clientDashboardController = App.app.getController('tpm.clientdashboard.ClientDashboard'); 
        var clientDashboard = Ext.ComponentQuery.query('clientdashboard')[0];
        var clientDashboardClientYearWindowChoose = button.up('clientdashboardclientyearchoosewindow');
        var clientTreeField =  clientDashboardClientYearWindowChoose.down('#ClientTreeField');
        var yearField = clientDashboardClientYearWindowChoose.down('#YearField');

        clientTreeField.validate();
        yearField.validate();

        var selectedClientRecord = clientTreeField.getRecord();
        var selectedYear = yearField.getValue();

        if (clientTreeField.isValid() && yearField.isValid()) {
            clientDashboard.show();
            var accountInformationButton = clientDashboard.down('#accountInformationButton');
            var promoWeeksButton = clientDashboard.down('#promoWeeksButton');
            var accountInformation = clientDashboard.down('accountinformation');

            clientDashboard['choosenClientTreeRecord'] = selectedClientRecord;
            clientDashboard['choosenYear'] = selectedYear;

            if (selectedClientRecord.data.IsOnInvoice != null) {
                if (selectedClientRecord.data.IsOnInvoice) {
                    accountInformation.down('#accountInformationClientType').setText('On Invoice');
                } else {
                    accountInformation.down('#accountInformationClientType').setText('Off Invoice');
                }
            } else { 
                accountInformation.down('#accountInformationClientType').setText('');
            }

            accountInformation.down('#accountInformationClientText').setText(selectedClientRecord.data.Name);
            accountInformation.down('#accountInformationYearText').setText(selectedYear);

            clientDashboardController.loadStoreWithFilters(clientDashboard, clientDashboardController.fillAccountInformationCallback, clientDashboardController.fillPromoWeeksCallback,false);
            button.up('window').close();
        }
    },

    getClientTreeStore: function () {
        var clientTreeStore = Ext.create('Ext.data.Store', {
            model: 'App.model.tpm.clienttree.ClientTree',
        });

        return clientTreeStore;
    },

    onTrigger1Click: function (picker) {
        var picker = picker.createPicker();
        var clientDashboardController = App.app.getController('tpm.clientdashboard.ClientDashboard');
        var clientTreeField = clientDashboardController.getClientTreeField(); 
        var clientTreeStore = clientDashboardController.getClientTreeStore();
        var clientTreeStoreProxy = clientTreeStore.getProxy();

        if (clientTreeField.getRecord()) {
            clientTreeStoreProxy.extraParams.clientObjectId = clientTreeField.getRecord().data.ObjectId;
        } else {
            clientTreeStoreProxy.extraParams.clientObjectId = null;
        }

        if (picker) {
            var clientTree = picker.down(this.selectorWidget);
            var clientTreeGrid = clientTree.down('basetreegrid');
            var clientTreeGridStore = clientTreeGrid.getStore();

            clientTree.chooseMode = true;
            if (clientTreeField.getRecord()) {
                clientTree.choosenClientObjectId = clientTreeField.getRecord().data.ObjectId;
            }

            clientTreeGridStore.addListener('load', function () {
                clientDashboardController.showCheckboxesForOnlyBaseClients(clientTreeGridStore.getRootNode().childNodes, clientTreeGrid.getChecked());
            });

            clientTreeGrid.addListener('checkchange', clientDashboardController.onClientTreeCheckChange);
            clientTree.down('basetreegrid').up('window').down('#select').addListener('click', function () { clientDashboardController.onSelectClientTreeInPicker(picker, clientTreeGrid.getChecked()[0]); });

            picker.show();

            var header = clientTree.getHeader();
            var splitter = clientTree.down('#splitter_1');
            var settings = clientTree.down('#clientTreeSettingsPanel');
            var addNodeButton = clientTree.down('#addNode');
            var deleteNodeButton = clientTree.down('#deleteNode');

            header.hide();
            splitter.hide();
            settings.hide();
            addNodeButton.hide();
            deleteNodeButton.hide();
        }
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

    onClientTreeCheckChange: function (item, checked, eOpts) {
        var clientDashboardController = App.app.getController('tpm.clientdashboard.ClientDashboard');
        var treegrid = item.store.ownerTree;
        var nodes = treegrid.getRootNode().childNodes;
        var clientTree = treegrid.up('clienttree');
        var editorForm = clientTree.down('editorform');
        var clientTreeController = App.app.getController('tpm.client.ClientTree');
        var clientChooseWindow = clientTree.up('window');
        var chooseButton = clientChooseWindow.down('#select');

        clientDashboardController.showCheckboxesForOnlyBaseClients(nodes);
        if (checked) {
            item.set('checked', true);
            chooseButton.enable();
        } else {
            chooseButton.disable();
        }
    },

    showCheckboxesForOnlyBaseClients: function (nodes, checkedNodes) {
        var me = this;
        if (nodes && nodes.length > 0) {
            nodes.forEach(function (node, index, array) {
                if (node.data.IsBaseClient) {
                    var nodeHtml = node.getOwnerTree().getView().getNode(node);
                    if (nodeHtml) {
                        Ext.fly(nodeHtml).addCls('hierarchy-baseclient');
                    }

                    if (checkedNodes && checkedNodes.length > 0 && checkedNodes[checkedNodes.length - 1].data.ObjectId == node.data.ObjectId) {
                        node.set('checked', true);
                    } else {
                        node.set('checked', false);
                    }
                } else {
                    node.set('checked', null);
                }

                me.showCheckboxesForOnlyBaseClients(node.childNodes, checkedNodes);
            });
        }
    },

    onSelectClientTreeInPicker: function (picker, clientTreeRecord) {
        var clientDashboardController = App.app.getController('tpm.clientdashboard.ClientDashboard');
        var clientTreeField = clientDashboardController.getClientTreeField();

        if (clientTreeRecord) {
            clientTreeField.setValue(new App.model.tpm.clienttree.ClientTree({
                Id: clientTreeRecord.data.Id,
                Name: clientTreeRecord.data.Name,
                ObjectId: clientTreeRecord.data.ObjectId,
                IsOnInvoice: clientTreeRecord.data.IsOnInvoice,

            }));
        }

        picker.close(); 
    },

    onClientTreePanelAfterRender: function (panel) {
        var clientDashboardController = App.app.getController('tpm.clientdashboard.ClientDashboard');
        panel.body.addListener('click', function () {
            clientDashboardController.onClientYearPanelClick(panel);
        });
    },

    onClientYearPanelClick: function (panel) {
        var clientDashboardController = App.app.getController('tpm.clientdashboard.ClientDashboard');
        var clientDashboard = panel.up('clientdashboard');
        var clientDashboardClientYearWindowChoose = Ext.widget('clientdashboardclientyearchoosewindow');
        var clientTreeField = clientDashboardController.getClientTreeField(clientDashboardClientYearWindowChoose);
        var yearField = clientDashboardClientYearWindowChoose.down('#YearField');

        var accountInformationYearText = panel.down('#accountInformationYearText');

        var clientTreeRecord = clientDashboard['choosenClientTreeRecord'];
        if (clientTreeRecord) {
            clientTreeField.setValue(new App.model.tpm.clienttree.ClientTree({
                Id: clientTreeRecord.data.Id,
                Name: clientTreeRecord.data.Name,
                ObjectId: clientTreeRecord.data.ObjectId,
                IsOnInvoice: clientTreeRecord.data.IsOnInvoice,
            }));
            clientDashboard['choosenClientTreeRecord'] = clientTreeRecord;
            clientDashboard['choosenYear'] = accountInformationYearText.text;
        }

        clientDashboardClientYearWindowChoose.down('#choose').addListener('click', clientDashboardController.onClientDashboardClientYearWindowChooseButtonClick);
        clientDashboardClientYearWindowChoose.show();

        yearField.setValue(accountInformationYearText.text);
    },

    fillAccountInformationCallback: function (records, clientDashboard, YtdYee) {
        var controller = App.app.getController('tpm.clientdashboard.ClientDashboard');
        var accountinformation = clientDashboard.down('accountinformation');
        var ROIPanel = clientDashboard.down('#ROIChartPanel');
        var incrementalNSV = clientDashboard.down('#panelIncPromoFirst2');
        var promoNSV = clientDashboard.down('#panelIncPromoSecond2');
        var shopperTIPlanPercent = 0;
        var shopperTIYTDPercent = 0;
        var shopperTIYEEPercent = 0;
        var shopperTIPlanMln = 0;
        var shopperTIYTDMln = 0;
        var shopperTIYEEMln = 0;
        var marketingTIPlanPercent = 0;
        var marketingTIYTDPercent = 0;
        var marketingTIYEEPercent = 0;
        var marketingTIPlanMln = 0;
        var marketingTIYTDMln = 0;
        var marketingTIYEEMln = 0;
        var productionPlanPercent = 0;
        var productionYTDPercent = 0;
        var productionYEEPercent = 0;
        var productionPlanMln = 0;
        var productionYTDMln = 0;
        var productionYEEMln = 0;
        var brandingPlanPercent = 0;
        var brandingYTDPercent = 0;
        var brandingYEEPercent = 0;
        var brandingPlanMln = 0;
        var brandingYTDMln = 0;
        var brandingYEEMln = 0;
        var btlPlanPercent = 0;
        var btlYTDPercent = 0;
        var btlYEEPercent = 0;
        var btlPlanMln = 0;
        var btlYTDMln = 0;
        var btlYEEMln = 0;
        var roiPlanPercent = 0;
        var roiYTDPercent = 0;
        var roiYEEPercent = 0;
        var lsvPlanMln = 0;
        var lsvYTDMln = YtdYee.YTD;
        var lsvYEEMln = YtdYee.YEE;
        var incrementalNSVPlanMln = 0;
        var incrementalNSVYTDMln = 0;
        var incrementalNSVYEEMln = 0;
        var promoNSVPlanMln = 0;
        var promoNSVYTDMln = 0;
        var promoNSVYEEMln = 0;

        var PromoTiCostPlanPercent = 0;
        var PromoTiCostPlan = 0;
        var PromoTiCostYTD = 0;
        var PromoTiCostYTDPercent = 0;
        var PromoTiCostYEE = 0;
        var PromoTiCostYEEPercent = 0;

        var NonPromoTiCostPlanPercent = 0;
        var NonPromoTiCostPlan = 0;
        var NonPromoTiCostYTD = 0;
        var NonPromoTiCostYTDPercent = 0;
        var NonPromoTiCostYEE = 0;
        var NonPromoTiCostYEEPercent = 0;

        var TotalPromoIncrementalEarnings = 0;
        var ActualPromoCost = 0;
        var TotalPromoCost = 0;
        var ActualPromoIncrementalEarnings = 0;

        //Берем первую, т.к. везде одинаковые значения
        if (records[0]) {
            NonPromoTiCostYTD = records[0].data.NonPromoTiCostYTD;
            NonPromoTiCostYEE = records[0].data.NonPromoTiCostYEE;
        }

        records.forEach(function (record) {
            shopperTIPlanPercent += record.data.ShopperTiPlanPercent;
            shopperTIYTDMln += record.data.ShopperTiYTD;
            shopperTIYEEMln += record.data.ShopperTiYEE;

            marketingTIPlanPercent += record.data.MarketingTiPlanPercent;

            productionPlanMln += record.data.ProductionPlan;
            productionYTDMln += record.data.ProductionYTD;
            productionYEEMln += record.data.ProductionYEE;

            brandingPlanMln += record.data.BrandingPlan;
            brandingYTDMln += record.data.BrandingYTD;
            brandingYEEMln += record.data.BrandingYEE;

            btlPlanMln += record.data.BTLPlan;
            btlYTDMln += record.data.BTLYTD;
            btlYEEMln += record.data.BTLYEE;

            roiPlanPercent += record.data.ROIPlanPercent;

            lsvPlanMln += record.data.LSVPlan;

            incrementalNSVPlanMln += record.data.IncrementalNSVPlan;
            incrementalNSVYTDMln += record.data.IncrementalNSVYTD;
            incrementalNSVYEEMln += record.data.IncrementalNSVYEE;

            promoNSVPlanMln += record.data.PromoNSVPlan;
            promoNSVYTDMln += record.data.PromoNSVYTD;
            promoNSVYEEMln += record.data.PromoNSVYEE;

            PromoTiCostPlanPercent += record.data.PromoTiCostPlanPercent;
            PromoTiCostYTD += record.data.PromoTiCostYTD;
            PromoTiCostYEE += record.data.PromoTiCostYEE;

            NonPromoTiCostPlanPercent += record.data.NonPromoTiCostPlanPercent;

            ActualPromoIncrementalEarnings += record.data.ActualPromoIncrementalEarnings;
            TotalPromoIncrementalEarnings += record.data.TotalPromoIncrementalEarnings;
            ActualPromoCost += record.data.ActualPromoCost;
            TotalPromoCost += record.data.TotalPromoCost;
        });

        shopperTIPlanPercent = shopperTIPlanPercent / records.length;
        marketingTIPlanPercent = marketingTIPlanPercent / records.length;
        roiPlanPercent = roiPlanPercent / records.length;

        PromoTiCostPlanPercent = PromoTiCostPlanPercent / records.length;
        NonPromoTiCostPlanPercent = NonPromoTiCostPlanPercent / records.length;

        shopperTIPlanMln = shopperTIPlanPercent * lsvPlanMln / 100;
        marketingTIPlanMln = marketingTIPlanPercent * lsvPlanMln / 100;

        PromoTiCostPlan = PromoTiCostPlanPercent * lsvPlanMln / 100;
        NonPromoTiCostPlan = NonPromoTiCostPlanPercent * lsvPlanMln / 100;

        productionPlanPercent = (lsvPlanMln != 0 ? (productionPlanMln / lsvPlanMln * 100) : 0);
        brandingPlanPercent = (lsvPlanMln != 0 ? (brandingPlanMln / lsvPlanMln * 100) : 0);
        btlPlanPercent = (lsvPlanMln != 0 ? (btlPlanMln / lsvPlanMln * 100) : 0);

        marketingTIYTDMln = PromoTiCostYTD + NonPromoTiCostYTD;
        marketingTIYEEMln = PromoTiCostYEE + NonPromoTiCostYEE;

        shopperTIYTDPercent = (lsvYTDMln != 0 ? (shopperTIYTDMln / lsvYTDMln * 100) : 0);
        marketingTIYTDPercent = (lsvYTDMln != 0 ? (marketingTIYTDMln / lsvYTDMln * 100) : 0);
        PromoTiCostYTDPercent = (lsvYTDMln != 0 ? (PromoTiCostYTD / lsvYTDMln * 100) : 0);
        NonPromoTiCostYTDPercent = (lsvYTDMln != 0 ? (NonPromoTiCostYTD / lsvYTDMln * 100) : 0);
        productionYTDPercent = (lsvYTDMln != 0 ? (productionYTDMln / lsvYTDMln * 100) : 0);
        brandingYTDPercent = (lsvYTDMln != 0 ? (brandingYTDMln / lsvYTDMln * 100) : 0);
        btlYTDPercent = (lsvYTDMln != 0 ? (btlYTDMln / lsvYTDMln * 100) : 0);
        roiYTDPercent = (ActualPromoCost != 0 ? (((ActualPromoIncrementalEarnings / ActualPromoCost) + 1) * 100) : 0);

        shopperTIYEEPercent = (lsvYEEMln != 0 ? (shopperTIYEEMln / lsvYEEMln * 100) : 0);
        marketingTIYEEPercent = (lsvYEEMln != 0 ? (marketingTIYEEMln / lsvYEEMln * 100) : 0);
        PromoTiCostYEEPercent = (lsvYEEMln != 0 ? (PromoTiCostYEE / lsvYEEMln * 100) : 0);
        NonPromoTiCostYEEPercent = (lsvYEEMln != 0 ? (NonPromoTiCostYEE / lsvYEEMln * 100) : 0);
        productionYEEPercent = (lsvYEEMln != 0 ? (productionYEEMln / lsvYEEMln * 100) : 0);
        brandingYEEPercent = (lsvYEEMln != 0 ? (brandingYEEMln / lsvYEEMln * 100) : 0);
        btlYEEPercent = (lsvYEEMln != 0 ? (btlYEEMln / lsvYEEMln * 100) : 0);
        roiYEEPercent = (TotalPromoCost != 0 ? (((TotalPromoIncrementalEarnings / TotalPromoCost) + 1) * 100) : 0);

        accountinformation.down('#shopperTIPlanPercent').originalValue = shopperTIPlanPercent.toFixed(2);
        accountinformation.down('#shopperTIYTDPercent').originalValue = shopperTIYTDPercent.toFixed(2);
        accountinformation.down('#shopperTIYEEPercent').originalValue = shopperTIYEEPercent.toFixed(2);
        accountinformation.down('#shopperTIPlanMln').originalValue = shopperTIPlanMln.toFixed(2);
        accountinformation.down('#shopperTIYTDMln').originalValue = shopperTIYTDMln.toFixed(2);
        accountinformation.down('#shopperTIYEEMln').originalValue = shopperTIYEEMln.toFixed(2);

        accountinformation.down('#marketingTIPlanPercent').originalValue = marketingTIPlanPercent.toFixed(2);
        accountinformation.down('#marketingTIYTDPercent').originalValue = marketingTIYTDPercent.toFixed(2);
        accountinformation.down('#marketingTIYEEPercent').originalValue = marketingTIYEEPercent.toFixed(2);
        accountinformation.down('#marketingTIPlanMln').originalValue = marketingTIPlanMln.toFixed(2);
        accountinformation.down('#marketingTIYTDMln').originalValue = marketingTIYTDMln.toFixed(2);
        accountinformation.down('#marketingTIYEEMln').originalValue = marketingTIYEEMln.toFixed(2);

        accountinformation.down('#productionPlanPercent').originalValue = productionPlanPercent.toFixed(2);
        accountinformation.down('#productionYTDPercent').originalValue = productionYTDPercent.toFixed(2);
        accountinformation.down('#productionYEEPercent').originalValue = productionYEEPercent.toFixed(2);
        accountinformation.down('#productionPlanMln').originalValue = productionPlanMln.toFixed(2);
        accountinformation.down('#productionYTDMln').originalValue = productionYTDMln.toFixed(2);
        accountinformation.down('#productionYEEMln').originalValue = productionYEEMln.toFixed(2);

        accountinformation.down('#brandingPlanPercent').originalValue = brandingPlanPercent.toFixed(2);
        accountinformation.down('#brandingYTDPercent').originalValue = brandingYTDPercent.toFixed(2);
        accountinformation.down('#brandingYEEPercent').originalValue = brandingYEEPercent.toFixed(2);
        accountinformation.down('#brandingPlanMln').originalValue = brandingPlanMln.toFixed(2);
        accountinformation.down('#brandingYTDMln').originalValue = brandingYTDMln.toFixed(2);
        accountinformation.down('#brandingYEEMln').originalValue = brandingYEEMln.toFixed(2);

        accountinformation.down('#btlPlanPercent').originalValue = btlPlanPercent.toFixed(2);
        accountinformation.down('#btlYTDPercent').originalValue = btlYTDPercent.toFixed(2);
        accountinformation.down('#btlYEEPercent').originalValue = btlYEEPercent.toFixed(2);
        accountinformation.down('#btlPlanMln').originalValue = btlPlanMln.toFixed(2);
        accountinformation.down('#btlYTDMln').originalValue = btlYTDMln.toFixed(2);
        accountinformation.down('#btlYEEMln').originalValue = btlYEEMln.toFixed(2);

        accountinformation.down('#lsvPlanMln').originalValue = lsvPlanMln.toFixed(2);
        accountinformation.down('#lsvYTDMln').originalValue = lsvYTDMln.toFixed(2);
        accountinformation.down('#lsvYEEMln').originalValue = lsvYEEMln.toFixed(2);

        accountinformation.down('#detailsButton').PromoTiCostPlanPercent = PromoTiCostPlanPercent;
        accountinformation.down('#detailsButton').PromoTiCostPlan = PromoTiCostPlan;
        accountinformation.down('#detailsButton').PromoTiCostYTD = PromoTiCostYTD;
        accountinformation.down('#detailsButton').PromoTiCostYTDPercent = PromoTiCostYTDPercent;
        accountinformation.down('#detailsButton').PromoTiCostYEE = PromoTiCostYEE;
        accountinformation.down('#detailsButton').PromoTiCostYEEPercent = PromoTiCostYEEPercent; 

        accountinformation.down('#detailsButton').NonPromoTiCostPlanPercent = NonPromoTiCostPlanPercent;
        accountinformation.down('#detailsButton').NonPromoTiCostPlan = NonPromoTiCostPlan;
        accountinformation.down('#detailsButton').NonPromoTiCostYTD = NonPromoTiCostYTD;
        accountinformation.down('#detailsButton').NonPromoTiCostYTDPercent = NonPromoTiCostYTDPercent;
        accountinformation.down('#detailsButton').NonPromoTiCostYEE = NonPromoTiCostYEE;
        accountinformation.down('#detailsButton').NonPromoTiCostYEEPercent = NonPromoTiCostYEEPercent; 

        controller.setValueFieldColor(accountinformation.down('#shopperTIYTDPercent'), shopperTIPlanPercent, shopperTIYTDPercent, accountinformation.down('#shopperTIYTDPercentArrow'));
        controller.setValueFieldColor(accountinformation.down('#shopperTIYEEPercent'), shopperTIPlanPercent, shopperTIYEEPercent, accountinformation.down('#shopperTIYEEPercentArrow'));
        controller.setValueFieldColor(accountinformation.down('#shopperTIYTDMln'), shopperTIPlanMln, shopperTIYTDMln, accountinformation.down('#shopperTIYTDMlnArrow'));
        controller.setValueFieldColor(accountinformation.down('#shopperTIYEEMln'), shopperTIPlanMln, shopperTIYEEMln, accountinformation.down('#shopperTIYEEMlnArrow'));

        controller.setValueFieldColor(accountinformation.down('#marketingTIYTDPercent'), marketingTIPlanPercent, marketingTIYTDPercent, accountinformation.down('#marketingTIYTDPercentArrow'));
        controller.setValueFieldColor(accountinformation.down('#marketingTIYEEPercent'), marketingTIPlanPercent, marketingTIYEEPercent, accountinformation.down('#marketingTIYEEPercentArrow'));
        controller.setValueFieldColor(accountinformation.down('#marketingTIYTDMln'), marketingTIPlanMln, marketingTIYTDMln, accountinformation.down('#marketingTIYTDMlnArrow'));
        controller.setValueFieldColor(accountinformation.down('#marketingTIYEEMln'), marketingTIPlanMln, marketingTIYEEMln, accountinformation.down('#marketingTIYEEMlnArrow'));

        controller.setValueFieldColor(accountinformation.down('#productionYTDPercent'), productionPlanPercent, productionYTDPercent, accountinformation.down('#productionYTDPercentArrow'));
        controller.setValueFieldColor(accountinformation.down('#productionYEEPercent'), productionPlanPercent, productionYEEPercent, accountinformation.down('#productionYEEPercentArrow'));
        controller.setValueFieldColor(accountinformation.down('#productionYTDMln'), productionPlanMln, productionYTDMln, accountinformation.down('#productionYTDMlnArrow'));
        controller.setValueFieldColor(accountinformation.down('#productionYEEMln'), productionPlanMln, productionYEEMln, accountinformation.down('#productionYEEMlnArrow'));

        controller.setValueFieldColor(accountinformation.down('#brandingYTDPercent'), brandingPlanPercent, brandingYTDPercent, accountinformation.down('#brandingYTDPercentArrow'));
        controller.setValueFieldColor(accountinformation.down('#brandingYEEPercent'), brandingPlanPercent, brandingYEEPercent, accountinformation.down('#brandingYEEPercentArrow'));
        controller.setValueFieldColor(accountinformation.down('#brandingYTDMln'), brandingPlanMln, brandingYTDMln, accountinformation.down('#brandingYTDMlnArrow'));
        controller.setValueFieldColor(accountinformation.down('#brandingYEEMln'), brandingPlanMln, brandingYEEMln, accountinformation.down('#brandingYEEMlnArrow'));

        controller.setValueFieldColor(accountinformation.down('#btlYTDPercent'), btlPlanPercent, btlYTDPercent, accountinformation.down('#btlYTDPercentArrow'));
        controller.setValueFieldColor(accountinformation.down('#btlYEEPercent'), btlPlanPercent, btlYEEPercent, accountinformation.down('#btlYEEPercentArrow'));
        controller.setValueFieldColor(accountinformation.down('#btlYTDMln'), btlPlanMln, btlYTDMln, accountinformation.down('#btlYTDMlnArrow'));
        controller.setValueFieldColor(accountinformation.down('#btlYEEMln'), btlPlanMln, btlYEEMln, accountinformation.down('#btlYEEMlnArrow'));

        controller.setupValuesAndTips()

        NSVChartFields = ['name', 'value'];
        var incPlan = (incrementalNSVPlanMln / 1000000).toFixed(1),
            incYTD = (incrementalNSVYTDMln / 1000000).toFixed(1),
            incYEE = (incrementalNSVYEEMln / 1000000).toFixed(1);

        var originalData = {
            Plan: incrementalNSVPlanMln,
            YTD: incrementalNSVYTDMln,
            YEE: incrementalNSVYEEMln
        }
        var incChartData = [
            { name: 'Plan', value: incPlan || 0 },
            { name: 'YTD', value: incYTD || 0 },
            { name: 'YEE', value: incYEE || 0 }
        ]
        if (incrementalNSV.items.length > 1) { // Больше 1, потому что там всегда есть 1 элемент – label
            var NSVChart = incrementalNSV.down('nsvchart');
            if (NSVChart) {
                NSVChart.destroy();
            }
            NSVChart = controller.createNSVChart(incrementalNSV, incChartData, NSVChartFields, originalData);
        } else {
            var NSVChart = controller.createNSVChart(incrementalNSV, incChartData, NSVChartFields, originalData);
        }

        var promoNSVPlan = (promoNSVPlanMln / 1000000).toFixed(1),
            promoNSVYTD = (promoNSVYTDMln / 1000000).toFixed(1),
            promoNSVYEE = (promoNSVYEEMln / 1000000).toFixed(1);

        originalData = {
            Plan: promoNSVPlanMln,
            YTD: promoNSVYTDMln,
            YEE: promoNSVYEEMln
        }
        var promoNSVChartData = [
            { name: 'Plan', value: promoNSVPlan || 0 },
            { name: 'YTD', value: promoNSVYTD || 0 },
            { name: 'YEE', value: promoNSVYEE || 0 }
        ]
        if (promoNSV.items.length > 1) {
            var promoNSVChart = promoNSV.down('nsvchart');
            if (promoNSVChart) {
                promoNSVChart.destroy();
            }
            controller.createNSVChart(promoNSV, promoNSVChartData, NSVChartFields, originalData);
        } else {
            controller.createNSVChart(promoNSV, promoNSVChartData, NSVChartFields, originalData);
        }

        var incrementalMlnLabel = accountinformation.down('#incrementalMlnLabel');
        var fixRaito = NSVChart.curWidth - NSVChart.series.items[0].bbox.width;
        incrementalMlnLabel.setWidth(incrementalMlnLabel.width + fixRaito);

        // ROI Chart
        var ROIChart = ROIPanel.down('roichart');
        if (ROIChart) {
            ROIChart.destroy();
        }
        var planROI = roiPlanPercent || 0,
            YTDROI = roiYTDPercent || 0,
            YEEROI = roiYEEPercent || 0;
        planROI = Ext.util.Format.round(planROI, 1);
        YTDROI = Ext.util.Format.round(YTDROI, 1);
        YEEROI = Ext.util.Format.round(YEEROI, 1);
        var maximum = Math.max(planROI, YTDROI, YEEROI) == 0 ? 100 : Math.max(planROI, YTDROI, YEEROI);
        maximum = maximum == 0 ? 100 : Math.ceil(maximum / 100) * 100;
        var ROIChartData = [
            { name: 'Plan', value: planROI },
            { name: 'YTD', value: YTDROI },
            { name: 'YEE', value: YEEROI },
        ];

        ROIPanel.add({
            xtype: 'roichart',
            store: Ext.create('Ext.data.Store', {
                storeId: 'roichartstore',
                fields: ['name', 'value'],
                data: ROIChartData
            }),
            width: '100%',
            height: '100%',
            maximum: maximum,
            showNegative: false,
        });
    },

    fillPromoWeeksCallback: function (records, clientDashboard) {
        var promoWeeks = clientDashboard.query('promoweeks')[0];
        promoWeeks.removePromoWeeksPanels();
        var promoWeeksPanels = [];
        var VodYEE, VodYTD, Weeks;
        records.forEach(function (record) {
            Weeks = record.data.PromoWeeks;
            VodYTD = (record.data.VodYTD * 100).toFixed(2);
            VodYEE = (record.data.VodYEE * 100).toFixed(2);
            if (Weeks != 0 || VodYTD != 0 || VodYEE != 0) {
                var promoWeeksPanel = Ext.widget('promoweekspanel', {
                    logoFileName: record.data.LogoFileName,
                    brandsegTechsubName: record.data.BrandsegTechsubName,
                    promoWeeks: Weeks,
                    vodYTD: VodYTD,
                    vodYEE: VodYEE
                });
                promoWeeksPanels.push(promoWeeksPanel);
            }
        });
        promoWeeks.addPromoWeeksPanels(promoWeeksPanels);
    },

    loadStoreWithFilters: function (clientDashboard, fillAccountInformationCallback, fillPromoWeeksCallback, refresh) {
        var clientKPIDataStore = Ext.create('App.store.core.DirectoryStore', {
            model: 'App.model.tpm.clientkpidata.ClientKPIData',
            autoLoad: false,
            sorters: [{
                property: 'BrandsegTechsubName',
                direction: 'ASC'
            }],
        })

        if (clientDashboard) {
            var filtersIds = ['ClientDashboardClientTreeObjectId', 'ClientDashboardYear'];

            var filters = [{
                property: 'ObjectId',
                operation: 'Equals',
                value: clientDashboard['choosenClientTreeRecord'].data.ObjectId
            }, {
                property: 'Year',
                operation: 'Equals',
                value: clientDashboard['choosenYear']
            }];

            clientKPIDataStore.setSeveralFixedFilters(filtersIds, filters, false);

            clientDashboard.setLoading(true);  
            clientKPIDataStore.on({
                load: {
                    fn: function (store, records) {
                        var params = {
                            year: clientDashboard['choosenYear'],
                            clientTreeId: clientDashboard['choosenClientTreeRecord'].data.ObjectId
                        }
                        if (params.clientTreeId) {
                            App.Util.makeRequestWithCallback('ClientDashboardViews', 'GetAllYEEF', params, function (data) {
                                var result = Ext.JSON.decode(data.httpResponse.data.value);
                                if (!refresh) {
                                    if (fillAccountInformationCallback) {
                                        fillAccountInformationCallback(records, clientDashboard, result);
                                    }
                                    if (fillPromoWeeksCallback) {
                                        fillPromoWeeksCallback(records, clientDashboard);
                                    }
                                    clientDashboard.setLoading(false);
                                } else {
                                    if (fillAccountInformationCallback && clientDashboard.down('#accountInformationButton').active) {
                                        fillAccountInformationCallback(records, clientDashboard, result);
                                    }
                                    if (fillPromoWeeksCallback && clientDashboard.down('#promoWeeksButton').active) {
                                        fillPromoWeeksCallback(records, clientDashboard, result);

                                    }
                                    clientDashboard.setLoading(false);
                                }
                            });
                        } else {
                            clientDashboard.setLoading(false);
                        }
                    },
                    single: true
                }
            });

            clientKPIDataStore.load();
        }
    },

    onAccountInformationButtonClick: function (button) {
        var clientDashboardFirstChildContainer = button.up('#clientDashboardFirstChildContainer');
        var promoWeeksButton = clientDashboardFirstChildContainer.down('#promoWeeksButton');
        var accountInformation = clientDashboardFirstChildContainer.down('#selectedContainer').down('accountinformation');
        var promoWeeks = clientDashboardFirstChildContainer.down('#selectedContainer').down('promoweeks');

        button.addCls('selected');
        button.addCls('client-dashboard-toolbar-button-selected');
        button.active = true;

        promoWeeksButton.removeCls('selected');
        promoWeeksButton.removeCls('client-dashboard-toolbar-button-selected');
        promoWeeksButton.active = false;

        promoWeeks.hide();
        accountInformation.show();
    },

    onPromoWeeksButtonClick: function (button) {
        var clientDashboardFirstChildContainer = button.up('#clientDashboardFirstChildContainer');
        var accountInformationButton = clientDashboardFirstChildContainer.down('#accountInformationButton');
        var accountInformation = clientDashboardFirstChildContainer.down('#selectedContainer').down('accountinformation');
        var promoWeeks = clientDashboardFirstChildContainer.down('#selectedContainer').down('promoweeks');

        button.addCls('selected');
        button.addCls('client-dashboard-toolbar-button-selected');
        button.active = true;

        accountInformationButton.removeCls('selected');
        accountInformationButton.removeCls('client-dashboard-toolbar-button-selected');
        accountInformationButton.active = false;

        accountInformation.hide();
        promoWeeks.show();
    },

    setValueFieldColor: function (valueField, planValue, value, arrowField) {
        if (planValue >= value) {
            valueField.removeCls('client-dashboard-account-panel-red-values');
            valueField.removeCls('client-dashboard-account-panel-blue-values');
            valueField.addCls('client-dashboard-account-panel-green-values');
            arrowField.removeCls('client-dashboard-account-panel-red-values');
            arrowField.removeCls('client-dashboard-account-panel-blue-values');
            arrowField.addCls('client-dashboard-account-panel-green-values');
            arrowField.setText('\u25B2');
        } else  {
            valueField.removeCls('client-dashboard-account-panel-green-values');
            valueField.removeCls('client-dashboard-account-panel-blue-values');
            valueField.addCls('client-dashboard-account-panel-red-values');
            arrowField.removeCls('client-dashboard-account-panel-green-values');
            arrowField.removeCls('client-dashboard-account-panel-blue-values');
            arrowField.addCls('client-dashboard-account-panel-red-values');
            arrowField.setText('\u25BC');            
        }
    },

    createNSVChart: function (container, data, fields, originalData) {
        var controller = App.app.getController('tpm.clientdashboard.ClientDashboard');
        var NSVChart = container.add({
            width: '64%',
            height: '95%',
            xtype: 'nsvchart',
            originalData: originalData,
            store: Ext.create('Ext.data.Store', {
                storeId: 'incrementalNSVstore',
                fields: fields,
                data: data,
            })
        });
        var minValue = Math.min(data[0].value, data[1].value, data[2].value);
        NSVChart.axes.items[0].maximum = Math.max(data[0].value, data[1].value, data[2].value);
        NSVChart.axes.items[0].minimum = minValue > 0 ? 0 : minValue;
        NSVChart.redraw();
        
        controller.setNSVChartZeroLine(NSVChart.axes.items[0]);
        controller.setNSVChartSeriesLabels(NSVChart);
        NSVChart.created = true;
        return NSVChart;
    },

    setNSVChartSeriesLabels: function (NSVChart) {
        // Удаляем старые, если есть
        if (NSVChart.series.seriesLabels) {
            NSVChart.series.seriesLabels.forEach(function (label) {
                NSVChart.surface.remove(label);
            });
        }
        
        var seriesLabels = [];
        NSVChart.series.items[0].items.forEach(function (item, index) {
            var chartAxes = NSVChart.axes.items[0],
                zeroY = chartAxes.chart.series.items[0].bounds.zero;
            
            var textValue = item.value[1];
            var textSprite = NSVChart.surface.add({
                type: 'text',
                fill: '#ffffff',
                text: textValue,
                x: item.attr.x + item.attr.width / 2,
                y: textValue < 0 ? zeroY + 15 : zeroY - 15,
                font: 'Bold 18px Arial',
            }).show(true); 
            if (textValue ===  '0.0') {
                NSVChart.surface.remove(textSprite);
            }
            else if (item.attr.height < textSprite.getBBox().height + 5) {
                textSprite.setAttributes({
                    translate: {
                        x: -textSprite.getBBox().width / 2,
                        y: -textSprite.getBBox().height / 2 - 5,
                    },
                    
                }, true);
                textSprite.setAttributes({
                        fill: item.attr.fill
                }, true);
                seriesLabels.push(textSprite);
            } else {
                textSprite.setAttributes({
                    translate: {
                        x: -textSprite.getBBox().width / 2,
                    }
                }, true);
                seriesLabels.push(textSprite);
            }
        });

        NSVChart.series.seriesLabels = seriesLabels;
    },

    setNSVChartZeroLine: function (chartAxes) {
        if (chartAxes.zeroLine) {
            chartAxes.chart.surface.remove(chartAxes.zeroLine);
        }

        var width = chartAxes.width,
            index = Ext.Array.indexOf(chartAxes.labels, 0) == -1 ? 0 : Ext.Array.indexOf(chartAxes.labels, 0),
            point = chartAxes.inflections[index],
            zeroY = chartAxes.chart.series.items[0].bounds.zero;
            path = ["M", point[0], zeroY, "l", width, 0],
        chartAxes.zeroLine = chartAxes.chart.surface.add({
            type: 'path',
            path: path,
            zIndex: 100,
            'stroke-width': '1px',
            stroke: '#d1d1ee'
        }).show(true);
    },

    setupValuesAndTips: function () {
        var elementsWithTips = Ext.ComponentQuery.query('*[valueField*=]');

        elementsWithTips.forEach(function (el) {
            var me = el;
            if (me.percent) {
                me.setText(me.originalValue == 0 ? 0 + "%" : me.originalValue > 10 ? parseFloat(me.originalValue).toFixed(0) + "%" : parseFloat(me.originalValue).toFixed(1) + "%");
            } else {
                me.setText((me.originalValue / 1000000) == 0 ? 0 : (parseFloat(me.originalValue / 1000000)).toFixed(1));
            }
            var value = me.originalValue == "" ? 0 : me.originalValue;
            if (me.fieldLabelTip) {
                me.fieldLabelTip.destroy();
            } 

            me.fieldLabelTip = Ext.create('Ext.tip.ToolTip', {
                target: me.el,
                preventLoseFocus: true,
                trackMouse: true,
                html: me.percent ? value + '%' : value,
                style: {
                    'background-color': 'rgba(63, 104, 149, 0.5)',
                    'border': 'none',
                },
                bodyStyle: 'color:#FFFFFF;'
            });
        })
    },
});