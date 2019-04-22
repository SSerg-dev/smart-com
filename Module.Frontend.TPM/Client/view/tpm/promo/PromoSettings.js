Ext.define('App.view.tpm.promo.PromoSettings', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.promosettings',

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
            minHeight: 122,
            flex: 1,
            items: [{
                xtype: 'fieldset',
                title: l10n.ns('tpm', 'Promo').value('CalendarPriority'),
                layout: {
                    type: 'vbox',
                    align: 'stretch',
                    pack: 'center'
                },
                items: [{
                    flex: 1,
                    xtype: 'sliderfield',
                    name: 'priority',
                    animate: false,
                    minValue: 1,
                    maxValue: 5,
                    padding: '15 4 0 4',
                    useTips: false,
                    crudAccess: ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'KeyAccountManager'],
                    listeners: {
                        change: function (slider, newValue, thumb, eOpts) {
                            if (newValue) {
                                slider.el.dom.querySelector('.x-slider-thumb').setAttribute('data-priority', newValue);
                                var promoEventButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step6]')[0];
                                promoEventButton.setText('<b>' + l10n.ns('tpm', 'promoStap').value('basicStep6') + '</b><br><p> Calendar priority: ' + newValue + '</p>');
                                promoEventButton.removeCls('notcompleted');
                                promoEventButton.setGlyph(0xf133);
                                promoEventButton.isComplete = true;

                                checkMainTab(promoEventButton.up(), slider.up('promoeditorcustom').down('button[itemId=btn_promo]'));
                            }
                        },
                        afterrender: function (slider, eOpts) {
                            var value = slider.getValue();

                            if (value) {
                                slider.el.dom.querySelector('.x-slider-thumb').setAttribute('data-priority', value);
                            }
                        },
                        render: function (slider) {
                            var innerEl = slider.getEl().down('.x-slider-inner');

                            slider.highlightEl = innerEl.appendChild({
                                tag: 'div',
                                cls: 'slider-highlightEl',
                                style: {
                                    backgroundColor: '#809DBA',
                                    height: '8px',
                                    position: 'relative'
                                }
                            });

                            slider.highlightEl.parent().select('.x-slider-thumb').setStyle('z-index', 10000);

                            setHighlightMargins(slider, slider.getValue());
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
                        html: '1',
                        cls: 'slider-left-lable'
                    }, {
                        xtype: 'label',
                        html: '5',
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
                    margin: '0 0 0 4',
                    html: [
                        "<span>",
                        "You can use <b>'View Priority'</b> level to change calendar priority on calendar view within the client. <b>Max priority is 1</b> (displays promo higher than others) and min priority is 5 (displays promo lower than others). <b>Default priority for all promos is 3.</b>",
                        "</span>",
                    ]
                }]
            }]
        }]
    }]
})