Ext.define('App.view.tpm.promo.PromoAdjustment', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.promoadjustment',

    needToSetHeight: true,

    bodyStyle: { "background-color": "#99A9B1" },

    items: [{
        xtype: 'container',
        cls: 'custom-promo-panel-container',
        layout: {
            type: 'hbox',
            align: 'stretchmax'
        },
        items: [{
            xtype: 'custompromopanel',
            minWidth: 245,
            minHeight: 195,
            flex: 1,
            items: [{
                xtype: 'fieldset',
                layout: {
                    type: 'vbox',
                    align: 'stretch',
                    pack: 'center'
                },
                items: [{
                    flex: 0.5,
                    margin: '30 10 0 10',
                    xtype: 'numberfield',
                    minValue: -100,
                    maxValue: 100,
                    allowBlank: false,
                    allowDecimals: false,
                    onAdjustmentChange: false,
                    needReadOnly: true,
                    fieldLabel: l10n.ns('tpm', 'Promo').value('Adjustment'),
                    name: 'Adjustment',
                    crudAccess: ['Administrator', 'SupportAdministrator', 'DemandPlanning', 'CustomerMarketing', 'KeyAccountManager'],
                    listeners: {
                        change: function (me, newValue, oldValue) {
                            newValue = newValue === null ? 0 : newValue;
                            if (Number.isInteger(newValue)) {
                                var deviationCoefficient = this.up('container').down('sliderfield[name=DeviationCoefficient]');
                                if (!deviationCoefficient.onSliderChange) {
                                    this.onAdjustmentChange = true;
                                    deviationCoefficient.setValue(-newValue);
                                    this.onAdjustmentChange = false;
                                }
                            }
                        }
                    }
                }, {
                    flex: 1,
                    xtype: 'sliderfield',
                    name: 'DeviationCoefficient',
                    minValue: -100,
                    maxValue: 100,
                    disabledCls: 'x-promo-slider-horz-disabled',
                    padding: '15 4 0 4',
                    minHeight: 40,
                    tipText: function (thumb) {
                        return Ext.String.format('{0}', -thumb.value);
                    },
                    onSliderChange: false,
                    crudAccess: ['Administrator', 'SupportAdministrator', 'DemandPlanning', 'CustomerMarketing', 'KeyAccountManager'],
                    listeners: {
                        change: function (me, newValue, thumb) {
                            this.onSliderChange = true;
                            var adjustment = this.up('container').down('numberfield[name=Adjustment]');
                            if (!adjustment.onAdjustmentChange)
                                adjustment.setValue(-newValue);
                            this.onSliderChange = false;

                            var promoAdjustmentButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step7]')[0];
                            promoAdjustmentButton.setText('<b>' + l10n.ns('tpm', 'promoStap').value('basicStep7') + '</b><br><p>' + l10n.ns('tpm', 'Promo').value('Adjustment') + ': ' + -newValue + '%' +'</p>');
                        },
                        render: function (slider) {
                            var value = slider.getValue();
                            value = Number.isInteger(value) ? value : 0;
                            this.setValue(value);

                            var promoAdjustmentButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step7]')[0];
                            promoAdjustmentButton.setText('<b>' + l10n.ns('tpm', 'promoStap').value('basicStep7') + '</b><br><p>' + l10n.ns('tpm', 'Promo').value('Adjustment') + ': ' + -value + '%' + '</p>');
                        },
                        beforechange: function (slider, newValue, oldValue) {
                            setHighlightMargins(slider, newValue);
                        }
                    }
                }, {
                    flex: 1,
                    xtype: 'panel',
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
        }, {
            xtype: 'splitter',
            itemId: 'splitter_6',
            cls: 'custom-promo-panel-splitter',
            collapseOnDblClick: false,
            listeners: {
                dblclick: {
                    fn: function (event, el) {
                        var cmp = Ext.ComponentQuery.query('splitter[itemId=splitter_6]')[0];
                        cmp.tracker.getPrevCmp().flex = 1;
                        cmp.tracker.getNextCmp().flex = 1;
                        cmp.ownerCt.updateLayout();
                    },
                    element: 'el'
                }
            }
        }, {
            xtype: 'custompromopanel',
            minWidth: 245,
            flex: 1,
            items: [{
                xtype: 'fieldset',
                title: 'Description',
                items: [{
                    xtype: 'image',
                    width: '47.1%',
                    margin: '5 5 5 5',
                    src: location.origin + '/Bundles/style/images/AdjustmentPromo_Graph_1.png',
                }, {
                    xtype: 'image',
                    width: '47.1%',
                    margin: '5 5 5 5',
                    src: location.origin + '/Bundles/style/images/AdjustmentPromo_Graph_2.png',
                }, {
                    margin: '0 0 0 4',
                    html: [
                        "<span>",
                        "You can use the slider to set distribution of shipments between weeks.Percent is for the first week of shipments.Subsequent shipments will be distributed in proportion to other weeks, keeping the original total promo volume.",
                        "</span>",
                    ]
                }]
            }]
        }]
    }]
})