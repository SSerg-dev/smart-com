Ext.define('App.view.tpm.client.DispatchDate', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.dispatchdate',

    items: [{
        xtype: 'container',
        html: l10n.ns('tpm', 'Dispatch').value('DispatchTitle'),
        style: 'margin: 10px 0 10px 0;'
    }, {
        xtype: 'container',
        style: 'border: 1px solid #ffffff; padding: 10px 6px; margin-bottom: 10px;',

        minHeight: 122,
        items: [{
            xtype: 'fieldcontainer',
            itemId: 'fieldcontainerStart',
            fieldLabel: l10n.ns('tpm', 'Dispatch').value('DispatchStart'),
            labelSeparator: ' ',
            width: '100%',
            labelWidth: 160,
            items: [{
                xtype: 'container',
                width: '100%',
                layout: {
                    type: 'hbox',
                    align: 'middle',
                },
                items: [{
                    xtype: 'booleancombobox',
                    name: 'IsBeforeStart',
                    store: {
                        type: 'booleanstore',
                        data: [
                            { id: null, text: '\u00a0' },
                            { id: true, text: l10n.ns('tpm', 'Dispatch').value('Before') },
                            { id: false, text: l10n.ns('tpm', 'Dispatch').value('After') }
                        ]
                    },
                    flex: 2,
                    style: 'margin-right: 5px;',
                    allowBlank: true
                }, {
                    xtype: 'numberfield',
                    name: 'DaysStart',
                    minValue: 1,
                    flex: 1,
                    style: 'margin-right: 5px',
                    disabled: true,
                    allowDecimals: false,
                    allowBlank: false
                }, {
                    xtype: 'booleancombobox',
                    name: 'IsDaysStart',
                    store: {
                        type: 'booleanstore',
                        data: [
                            { id: true, text: l10n.ns('tpm', 'Dispatch').value('Days') },
                            { id: false, text: l10n.ns('tpm', 'Dispatch').value('Weeks') }
                        ],
                    },
                    flex: 2,
                    disabled: true,
                    allowBlank: false,
                }]
            }]
        }, {
            xtype: 'fieldcontainer',
            itemId: 'fieldcontainerEnd',
            fieldLabel: l10n.ns('tpm', 'Dispatch').value('DispatchEnd'),
            labelSeparator: ' ',
            width: '100%',
            labelWidth: 160,
            style: 'margin-bottom: 0', 
            items: [{
                xtype: 'container',
                layout: {
                    type: 'hbox',
                    align: 'middle'
                },
                items: [{
                    xtype: 'booleancombobox',
                    name: 'IsBeforeEnd',
                    store: {
                        type: 'booleanstore',
                        data: [
                            { id: null, text: '\u00a0' },
                            { id: true, text: l10n.ns('tpm', 'Dispatch').value('Before') },
                            { id: false, text: l10n.ns('tpm', 'Dispatch').value('After') }
                        ]
                    },
                    flex: 2,
                    style: 'margin-right: 5px',
                    allowBlank: true
                }, {
                    xtype: 'numberfield',
                    name: 'DaysEnd',
                    minValue: 1,
                    flex: 1,
                    style: 'margin-right: 5px;',
                    disabled: true,
                    allowDecimals: false,
                    allowBlank: false,
                }, {
                    xtype: 'booleancombobox',
                    name: 'IsDaysEnd',
                    store: {
                        type: 'booleanstore',
                        data: [
                            { id: true, text: l10n.ns('tpm', 'Dispatch').value('Days') },
                            { id: false, text: l10n.ns('tpm', 'Dispatch').value('Weeks') }
                        ]
                    },
                    flex: 2,
                    disabled: true,
                    allowBlank: false,
                }]
            }]
        }, {
            xtype: 'fieldcontainer',
            itemId: 'fieldcontainerAdjustmentP',
            fieldLabel: l10n.ns('tpm', 'ClientTree').value('Adjustment'),
            labelSeparator: ' ',
            width: '100%',
            labelWidth: 160,
            layout: {
                type: 'hbox',
                align: 'middle'
            },
            style: 'margin-bottom: 0; margin-top: 10px;', 
            items: [{
                xtype: 'numberfield',
                name: 'Adjustment',
                width: '92.3%',
                minValue: -100,
                maxValue: 100,
                allowDecimals: false,
                onAdjustmentChange: false,
                listeners: {
                    change: function (me, newValue, oldValue) {
                        newValue = newValue === null ? 0 : newValue;
                        if (Number.isInteger(newValue)) {
                            var deviationCoefficient = this.up('container').up('container').down('sliderfield[name=DeviationCoefficient]');
                            if (!deviationCoefficient.onSliderChange) {
                                this.onAdjustmentChange = true;
                                deviationCoefficient.setValue(-newValue);
                                this.onAdjustmentChange = false;
                            }
                        }
                    }
                }
            }]
        }, {
            xtype: 'fieldcontainer',
            itemId: 'fieldcontainerAdjustment',
            width: '95%',
            layout: {
                type: 'hbox',
                align: 'middle'
            },
            style: 'margin-bottom: 0',
            items: [{
                xtype: 'sliderfield',
                minHeight: 40,
                flex: 1,
                margin: '0 -7 0 -7',
                width: '100%',
                disabledCls: 'x-slider-horz-disabled',
                name: 'DeviationCoefficient',
                minValue: -100,
                maxValue: 100,
                tipText: function (thumb) {
                    return Ext.String.format('{0}', -thumb.value);
                },
                onSliderChange: false,
                listeners: {
                    change: function (me, newValue, thumb) {
                        this.onSliderChange = true;
                        var adjustment = this.up('container').up('container').down('numberfield[name=Adjustment]');
                        if (!adjustment.onAdjustmentChange)
                            adjustment.setValue(-newValue);
                        this.onSliderChange = false;
                    },
                    render: function (me) {
                        this.setValue(-me.getValue());
                    }
                }
            }]
        }, {
            xtype: 'panel',
            flex: 1,
            minHeight: 17,
            style: 'background-color: #eeeeee !important;',
            cls: 'adjustment-custom-panel',
            items: [{
                xtype: 'label',
                html: '\n +100%',
                cls: 'slider-left-lable'
            }, {
                xtype: 'label',
                html: '\n 0',
                cls: 'slider-center-lable'
            }, {
                xtype: 'label',
                html: '\n -100%',
                cls: 'slider-right-lable'
            }]
        }]
    }]
});