Ext.define('App.view.tpm.clientdashboard.PromoWeeksPanel', {
    extend: 'Ext.container.Container',
    alias: 'widget.promoweekspanel',
    cls: 'promo-weeks-panel',

    height: 60,
    width: '100%',

    data: {
        logoFileName: null,
        brandTechName: null,
        promoWeeks: null,
        vodYTD: null,
        vodYEE: null
    },

    layout: {
        type: 'hbox',
        align: 'stretch'
    },

    items: [{
        xtype: 'container',
        width: '100%',
        layout: {
            type: 'hbox',
            align: 'stretch'
        },
        items: [{
            xtype: 'container',
            width: '100%',
            layout: {
                type: 'vbox',
                pack: 'center',
            },
            items: [{
                xtype: 'container',
                cls: 'client-dashboard-promo-weeks-panel-logo-container',
                width: '100%',
                layout: 'fit',
                items: [{
                    xtype: 'image',
                    itemId: 'logoFileNameValue',
                    cls: 'client-dashboard-promo-weeks-panel-logo',
                    listeners: {
                        render: function(el) {
                            el.imgEl.on('error', function () {
                                el.setSrc('/bundles/style/images/swith-glyph-gray.png');
                            });
                        }
                    },
                }],
            }],
            flex: 1,
        }, {
            xtype: 'container',
            width: '100%',
            layout: {
                type: 'vbox',
                pack: 'center',
            },
            items: [{
                xtype: 'container',
                width: '100%',
                items: [{
                    xtype: 'container',
                    html: l10n.ns('tpm', 'ClientDashboardPromoWeeksPanel').value('BrandTechName'),
                    itemId: 'brandTechNameLable',
                    cls: 'client-dashboard-promo-weeks-panel-lable',
                }, {
                    xtype: 'container',
                    itemId: 'brandTechNameValue',
                    cls: 'client-dashboard-promo-weeks-panel-value',
                }],
            }],
            flex: 2,
        }, {
            xtype: 'container',
            width: '100%',
            layout: {
                type: 'vbox',
                pack: 'center',
            },
            items: [{
                xtype: 'container',
                width: '100%',
                items: [{
                    xtype: 'container',
                    html: l10n.ns('tpm', 'ClientDashboardPromoWeeksPanel').value('PromoWeeks'),
                    itemId: 'promoWeeksLable',
                    cls: 'client-dashboard-promo-weeks-panel-lable',
                }, {
                    xtype: 'container',
                    itemId: 'promoWeeksValue',
                    cls: 'client-dashboard-promo-weeks-panel-value',
                }],
            }],
            flex: 2,
        }, {
            xtype: 'container',
            width: '100%',
            layout: {
                type: 'vbox',
                pack: 'center',
            },
            items: [{
                xtype: 'container',
                width: '100%',
                items: [{
                    xtype: 'container',
                    html: l10n.ns('tpm', 'ClientDashboardPromoWeeksPanel').value('VodYtd'),
                    itemId: 'vodYtdLable',
                    cls: 'client-dashboard-promo-weeks-panel-lable',
                }, {
                    xtype: 'container',
                    itemId: 'vodYtdValue',
                    cls: 'client-dashboard-promo-weeks-panel-value',
                }],
            }],
            flex: 2,
        }, {
            xtype: 'container',
            width: '100%',
            layout: {
                type: 'vbox',
                pack: 'center',
            },
            items: [{
                xtype: 'container',
                width: '100%',
                items: [{
                    xtype: 'container',
                    html: l10n.ns('tpm', 'ClientDashboardPromoWeeksPanel').value('VodYee'),
                    itemId: 'vodYeeLable',
                    cls: 'client-dashboard-promo-weeks-panel-lable',
                }, {
                    xtype: 'container',
                    itemId: 'vodYeeValue',
                    cls: 'client-dashboard-promo-weeks-panel-value',
                }],
            }],
            flex: 2,
        }],
    }],

    constructor: function (config) {
        this.callParent(arguments);

        this.data.logoFileName = config.logoFileName;
        this.data.brandTechName = config.brandTechName;
        this.data.promoWeeks = config.promoWeeks;
        this.data.vodYTD = config.vodYTD;
        this.data.vodYEE = config.vodYEE;

        var pattern = '/odata/ProductTrees/DownloadLogoFile?fileName={0}';

        if (this.data.logoFileName) {
            var downloadFileUrl = document.location.href + Ext.String.format(pattern, this.data.logoFileName);
            this.down('#logoFileNameValue').setSrc(downloadFileUrl);
        } else {
            this.down('#logoFileNameValue').setSrc('/bundles/style/images/swith-glyph-gray.png');
        }

        this.down('#brandTechNameValue').html = this.data.brandTechName ? this.data.brandTechName : '-';
        this.down('#promoWeeksValue').html = this.data.promoWeeks ? this.data.promoWeeks : '-';
        this.down('#vodYtdValue').html = this.data.vodYTD ? this.data.vodYTD : '-';
        this.down('#vodYeeValue').html = this.data.vodYEE ? this.data.vodYEE : '-';
    }
});