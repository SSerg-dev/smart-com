Ext.define('App.view.tpm.promo.PromoBudgetYear', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.promobudgetyear',


    items: [{
        xtype: 'container',
        cls: 'custom-promo-panel-container',
        layout: {
            type: 'hbox',
            align: 'stretchmax'
        },
        items: [{
            xtype: 'custompromopanel',
            name: 'chooseBudgetYear',
            minWidth: 245,
            minHeight: 86,
            flex: 1,
            items: [{
                xtype: 'fieldset',
                title: 'Choose year',
                padding: '1 4 0 4',
                items: [{
                    xtype: 'form',
                    layout: {
                        type: 'vbox',
                        layoutConfig: {
                            pack: 'center',
                            align: 'middle',
                        },
                    },
                        items: [{
                            xtype: 'combobox',
                            name: 'PromoBudgetYear',
                            width: '100%',
                            id: 'budgetYearCombo',
                            valueField: 'year',
                            displayField: 'year',
                            labelWidth: '100%',
                            queryMode: 'local',
                            editable: false,
                            padding: '5 5 5 5',
                            crudAccess: ['Administrator', 'SupportAdministrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager', 'DemandFinance', 'DemandPlanning'],
                            fieldLabel: l10n.ns('tpm', 'Promo').value('BudgetYear'),
                            store: Ext.create('Ext.data.Store', {
                                fields: ['year']
                            }),
                            listeners: {
                                change: function (field, newValue, oldValue) {
                                    if (newValue) {
                                        var promoBudgetYearButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step5]')[0];
                                        promoBudgetYearButton.setText('<b>' + l10n.ns('tpm', 'promoStap').value('basicStep5') + '</b><br><p> ' + newValue + '</p>');
                                        promoBudgetYearButton.removeCls('notcompleted');
                                        promoBudgetYearButton.setGlyph(0xf133);
                                        promoBudgetYearButton.isComplete = true;
                                    }
                                    else {
                                        var promoBudgetYearButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step5]')[0];
                                        promoBudgetYearButton.setText('<b>' + l10n.ns('tpm', 'promoStap').value('basicStep5') + '</b><br><p></p>');
                                        promoBudgetYearButton.addCls('notcompleted');
                                        promoBudgetYearButton.setGlyph(0xf130);
                                        promoBudgetYearButton.isComplete = false;
                                    }
                                }
                            }
                        }]
                }]
            }]
        }, {
            xtype: 'splitter',
            itemId: 'splitter_5',
            cls: 'custom-promo-panel-splitter',
            collapseOnDblClick: false,
            listeners: {
                dblclick: {
                    fn: function (event, el) {
                        var cmp = Ext.ComponentQuery.query('splitter[itemId=splitter_5]')[0];
                        cmp.tracker.getPrevCmp().flex = 1;
                        cmp.tracker.getNextCmp().flex = 1;
                        cmp.ownerCt.updateLayout();
                    },
                    element: 'el'
                }
            }
        }, {
            xtype: 'custompromopanel',
            itemId: 'promoBudgetYearDescription',
            minWidth: 245,
            flex: 1,
            items: [{
                xtype: 'fieldset',
                title: 'Description',
                items: [{
                    xtype: 'label',
                    itemId: 'promoBudgetYearDescription',
                    margin: '0 0 0 4',
                    html: [
                        "<span>",
                        "The fiscal year to which Shopper TE are related due to accounting rules (not start date only). Invoice for the activity will also be dated to Budget Year (Shopper TE) selected in this field.",
                        "</span>",
                    ]
                }]
            }]
        }]
    }]
})