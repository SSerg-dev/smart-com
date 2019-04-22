Ext.define('App.view.core.common.EditableDetailForm', {
    extend: 'Core.form.editabledetail.BaseEditableDetailForm',
    alias: 'widget.editabledetailform',

    modeToolbarMap: {
        'empty': 'readtoolbar',
        'loaded': 'readtoolbar',
        'creating': 'createtoolbar',
        'editing': 'edittoolbar',
        'created': 'createtoolbar',
        'changed': 'edittoolbar'
    },

    spotlightModes: [
        Core.BaseEditableDetailForm.CREATING_MODE,
        Core.BaseEditableDetailForm.EDITING_MODE,
        Core.BaseEditableDetailForm.CREATED_MODE,
        Core.BaseEditableDetailForm.CHANGED_MODE
    ],

    dockedItems: [{
        xtype: 'panel',
        ui: 'detailform-panel',
        dock: 'top',
        height: 35,

        layout: {
            type: 'hbox',
            pack: 'center',
            align: 'middle'
        },

        items: [{
            xtype: 'label',
            itemId: 'formtitle',
            text: l10n.ns('core').value('editingModeTitle'),
            cls: 'x-form-item-label',
            hidden: true
        }]
    }, {
        xtype: 'container',
        dock: 'bottom',
        layout: 'card',

        defaults: {
            xtype: 'toolbar',
            ui: 'footer',

            layout: {
                type: 'hbox',
                pack: 'center'
            },

            defaults: {
                ui: 'white-button-footer-toolbar',
                width: 135
            }
        },

        items: [{
            itemId: 'readtoolbar',
            items: [{
                itemId: 'prev',
                text: l10n.ns('core', 'selectablePanelButtons').value('prev'),
                glyph: 0xf04d,
                margin: '5 10 5 0'
            }, {
                itemId: 'next',
                text: l10n.ns('core', 'selectablePanelButtons').value('next'),
                glyph: 0xf054,
                iconAlign: 'right',
                margin: '5 0 5 0'
            }]
        }, {
            itemId: 'createtoolbar',
            items: [{
                itemId: 'cancel',
                text: l10n.ns('core', 'createWindowButtons').value('cancel'),
                margin: '5 10 5 0'
            }, {
                itemId: 'ok',
                text: l10n.ns('core', 'createWindowButtons').value('ok'),
                ui: 'green-button-footer-toolbar',
                margin: '5 0 5 0'
            }]
        }, {
            itemId: 'edittoolbar',
            items: [{
                itemId: 'cancel',
                text: l10n.ns('core', 'buttons').value('cancel'),
                margin: '5 10 5 0'
            }, {
                itemId: 'ok',
                text: l10n.ns('core', 'buttons').value('ok'),
                ui: 'green-button-footer-toolbar',
                margin: '5 0 5 0'
            }]
        }]
    }],

    afterRender: function () {
        var me = this,
            container = me.up('[hasScroll=true]');

        me.callParent(arguments);

        if (container) {
            var targetEl = container.getTargetEl();
            $(targetEl.dom).bind(
                'jsp-will-scroll-y',
                function (event, scrollPositionY) {
                    if (me.spotlight && me.spotlight.isVisible() && targetEl.id == event.target.id) {
                        return false;
                    }
                }
            );
        }
        // в мастер-дитэйле блокируем прокрутку в общем контейнере
        var upperContainer = container.up('[id = viewcontainer][hasScroll=true]');
        if (upperContainer) {
            var targetEl = upperContainer.getTargetEl();
            $(targetEl.dom).bind(
                'jsp-will-scroll-y',
                function (event, scrollPositionY) {
                    if (me.spotlight && me.spotlight.isVisible() && targetEl.id == event.target.id) {
                        return false;
                    }
                }
            );
        }
    },

    afterFormShow: function () {
        this.down('field').focus(true, 10);
    },

    onModeChange: function (newMode, oldMode) {
        var toolbarId = this.modeToolbarMap[newMode],
            formTitle = this.down('#formtitle');

        if (toolbarId) {
            this.getBottomToolbar().getLayout().setActiveItem(toolbarId);
        }

        if (Ext.Array.contains(this.spotlightModes, newMode)) {
            this.getSpotlight().show();
            formTitle.show();
        } else if (this.spotlight && this.spotlight.isVisible()) {
            formTitle.hide();
            this.spotlight.hide();
        }
    },

    getBottomToolbar: function () {
        return this.getDockedItems('[dock=bottom]')[0];
    },

    getSpotlight: function () {
        var spot = this.spotlight;

        if (!spot) {
            spot = this.spotlight = Ext.create('Ext.ux.ActiveSpotlight', {
                target: this,
                constraint: this.getSpotlightConstraintArea()
            });
            spot.on('click', this.onSpotlightClick, this);
        }

        return spot;
    },

    getSpotlightConstraintArea: function () {
        var container = this.up('#viewcontainer, window'),
            region = container.getRegion();

        if (container.is('#viewcontainer')) {
            var header = container.up('viewport').down('#header').getEl().first();
            var curtainHeight = Math.abs(header.getRegion().bottom - region.top);
            region = region.adjust(curtainHeight, 0, 0, 0);
        }

        return region;
    },

    onSpotlightClick: function () {
        Ext.Msg.show({
            title: l10n.ns('core').value('confirmTitle'),
            msg: l10n.ns('core').value('cancelEditMsg'),
            fn: onMsgBoxClose,
            scope: this,
            icon: Ext.Msg.QUESTION,
            buttons: Ext.Msg.YESNO
        });

        function onMsgBoxClose(buttonId) {
            if (buttonId === 'yes') {
                var panel = this.up('combineddirectorypanel');
                if (panel) {
                    var layout = panel.getLayout();

                    this.rejectChanges();
                    if (panel.lastActiveItemId && layout.getActiveItem() !== panel.lastActiveItemId) {
                        layout.setActiveItem(panel.lastActiveItemId);
                    }
                }
            }
        }
    },

    onDestroy: function () {
        Ext.destroy(this.spotlight);
        delete this.spotlight;
        this.callParent(arguments);
    }

});