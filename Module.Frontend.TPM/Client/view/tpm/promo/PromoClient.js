﻿Ext.define('App.view.tpm.promo.PromoClient', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.promoclient',

    // запись клиента
    clientTreeRecord: null,
    // блокировка дат для промо в некоторых статусах
    treesChangingBlockDate: false,

    items: [{
        xtype: 'container',
        cls: 'custom-promo-panel-container',
        layout: {
            type: 'hbox',
            align: 'stretchmax'
        },
        items: [{
            xtype: 'custompromopanel',
            name: 'chooseClient',
            minWidth: 245,
            height: 202,
            flex: 1,
            layout: {
                type: 'vbox',
                align: 'stretch',
                pack: 'center'
            },
            items: [{
                xtype: 'fieldset',
                title: l10n.ns('tpm', 'PromoClient').value('ChooseClient'),
                padding: '1 4 0 4',
                height: 154,
                items: [{
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [{
                        xtype: 'container',                        
                        padding: '0 5 0 7',
                        items: [{
                            xtype: 'button',
                            itemId: 'choosePromoClientBtn',
                            glyph: 0xf968,
                            scale: 'large',
                            height: 98,
                            width: 110,
                            text: '<b>' + l10n.ns('tpm', 'PromoClient').value('ChooseClient') + '<br/>...</b>',
                            iconAlign: 'top',                            
                            cls: 'custom-event-button promobasic-choose-btn',
                            disabledCls: 'promobasic-choose-btn-disabled',
                        }]
                    }, {
                        xtype: 'container',
                        flex: 1,
                        padding: '0 5 0 7',
                        items: [{
                            xtype: 'singlelinedisplayfield',
                            name: 'PromoClientName',
                            width: '100%',
                            fieldLabel: l10n.ns('tpm', 'ClientTree').value('Name'),
                        }, {
                            xtype: 'singlelinedisplayfield',
                            name: 'PromoClientType',
                            width: '100%',
                            fieldLabel: l10n.ns('tpm', 'ClientTree').value('Type')
                        }, {
                            xtype: 'singlelinedisplayfield',
                            name: 'PromoClientRetailType',
                            width: '100%',
                            fieldLabel: l10n.ns('tpm', 'RetailType').value('Name')
                        }]
                    }]
                }]
            }, {
                xtype: 'container',
                height: 33,
                flex: 1,
                layout: {
                    type: 'hbox',
                    align: 'top',
                    pack: 'center'
                },
                items: [{
                    xtype: 'tbspacer',
                    flex: 1
                }, {
                    xtype: 'button',
                    width: 111,
                    padding: '3 7 3 10',
                    cls: 'hierarchyButton hierarchyButtonList',
                    itemId: 'outletList',
                    text: l10n.ns('tpm', 'PromoClient').value('OutletsList'),
                    tooltip: 'Outlets list',
                    iconCls: 'icon-list-png'
                }]
            }]
        }, {
            xtype: 'splitter',
            itemId: 'splitter_chooseClient',
            cls: 'custom-promo-panel-splitter',
            collapseOnDblClick: false,
            listeners: {
                dblclick: {
                    fn: function (event, el) {
                        var cmp = Ext.ComponentQuery.query('splitter#splitter_chooseClient')[0];
                        cmp.tracker.getPrevCmp().flex = 1;
                        cmp.tracker.getNextCmp().flex = 1;
                        cmp.ownerCt.updateLayout();
                    },
                    element: 'el'
                }
            }
        }, {
            xtype: 'custompromopanel',
            name: 'PromoClientSettings',
            minWidth: 245,
            height: 202,
            flex: 1,
            layout: {
                type: 'vbox',
                align: 'stretch',
                pack: 'center'
            },
            items: [{
                xtype: 'fieldset',
                title: l10n.ns('tpm', 'PromoClient').value('Settings'),
                padding: '0 10 0 10',
                height: 154,
                defaults: {
                    margin: '5 0 0 0',
                },
                items: [{
                    xtype: 'singlelinedisplayfield',
                    name: 'PromoClientObjectId',
                    width: '100%',
                    labelWidth: 160,
                    fieldLabel: l10n.ns('tpm', 'ClientTree').value('ClientHierarchyCode')
                }, {
                    xtype: 'singlelinedisplayfield',
                    name: 'PromoClientOutletCount',
                    width: '100%',
                    labelWidth: 160,
                    fieldLabel: l10n.ns('tpm', 'ClientTree').value('OutletCount')
                }, {
                    xtype: 'singlelinedisplayfield',
                    name: 'PromoClientPPEW1',
                    width: '100%',
                    labelWidth: 160,
                    fieldLabel: l10n.ns('tpm', 'ClientTree').value('PostPromoEffectW1')
                }, {
                    xtype: 'singlelinedisplayfield',
                    name: 'PromoClientPPEW2',
                    width: '100%',
                    labelWidth: 160,
                    fieldLabel: l10n.ns('tpm', 'ClientTree').value('PostPromoEffectW2')
                }]
            }, {
                xtype: 'container',
                height: 33,
                flex: 1,
                layout: {
                    type: 'hbox',
                    align: 'top',
                    pack: 'center'
                },
                items: [{
                    xtype: 'tbspacer',
                    flex: 1
                }, {
                    xtype: 'button',
                    itemId: 'promoClientSettignsBtn',
                    width: 90,
                    padding: '3 7 3 10',
                    cls: 'hierarchyButton hierarchyButtonList',                    
                    text: l10n.ns('tpm', 'PromoClient').value('Settings'),
                    tooltip: l10n.ns('tpm', 'PromoClient').value('Settings'),
                    glyph: 0xf8de
                }]
            }]
        }]
    }],

    // заполнение формы
    fillForm: function (clientTreeRecord, treesChangingBlockDate) {
        var chooseBtn = this.down('#choosePromoClientBtn');

        this.clientTreeRecord = clientTreeRecord;
        this.treesChangingBlockDate = treesChangingBlockDate;

        if (this.clientTreeRecord) {
            var iconSrc = this.clientTreeRecord.LogoFileName ? '/odata/ClientTrees/DownloadLogoFile?fileName=' + encodeURIComponent(this.clientTreeRecord.LogoFileName) : '/bundles/style/images/swith-glyph-gray.png';

            chooseBtn.setText('<b>' + clientTreeRecord.Name + '<br/>...</b>');
            chooseBtn.setGlyph();
            chooseBtn.setIcon(iconSrc);
            chooseBtn.setIconCls('promoClientChooseBtnIcon'); 
            
            this.down('[name=PromoClientName]').setValue(clientTreeRecord.Name);
            this.down('[name=PromoClientType]').setValue(clientTreeRecord.Type);
            this.down('[name=PromoClientRetailType]').setValue(clientTreeRecord.RetailTypeName);
            this.down('[name=PromoClientObjectId]').setValue(clientTreeRecord.ObjectId);
            //this.down('[name=PromoClientOutletCount]').setValue(record.data.PromoClientName);
            this.down('[name=PromoClientPPEW1]').setValue(clientTreeRecord.PostPromoEffectW1);
            this.down('[name=PromoClientPPEW2]').setValue(clientTreeRecord.PostPromoEffectW2);

        } else {
            chooseBtn.setText('<b>' + l10n.ns('tpm', 'PromoClient').value('ChooseClient') + '<br/>...</b>');            
            chooseBtn.setIcon();
            chooseBtn.setIconCls('x-btn-glyph materialDesignIcons');           
            chooseBtn.setGlyph(0xf968);            

            this.down('[name=PromoClientName]').setValue(null);
            this.down('[name=PromoClientType]').setValue(null);
            this.down('[name=PromoClientRetailType]').setValue(null);
            this.down('[name=PromoClientObjectId]').setValue(null);
            //this.down('[name=PromoClientOutletCount]').setValue(record.data.PromoClientName);
            this.down('[name=PromoClientPPEW1]').setValue(null);
            this.down('[name=PromoClientPPEW2]').setValue(null);
        }

        chooseBtn.fireEvent('resize', chooseBtn); // для обновления отрисовки кнопки

    },

    // показать параметры клиента
    showSettings: function () {
        var settingsWind = Ext.create('App.view.tpm.promo.PromoClientSettingsWindow');
        settingsWind.show();

        if (this.clientTreeRecord)
            settingsWind.fillForm(this.clientTreeRecord);
    },

    // вызвать форму выбора клиента
    chooseClient: function (callBackChooseFnc) {
        var clientObjectId = this.clientTreeRecord ? this.clientTreeRecord.ObjectId : null;
        var blockDate = this.treesChangingBlockDate;

        var choosePromoClientWind = Ext.create('App.view.tpm.promo.PromoClientChooseWindow', {
            choosenClientObjectId: clientObjectId,
            treesChangingBlockDate: blockDate,
            callBackChooseFnc: callBackChooseFnc
        });

        choosePromoClientWind.show();
    }
})