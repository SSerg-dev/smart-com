Ext.define('App.view.core.common.CustomHeaderMenu', {
    extend: 'Ext.menu.Menu',
    alias: 'widget.customheadermenu',

    afterLayout: function (layout) {
        this.callParent(arguments);

        if (!this.widthCorrected) {
            // Prevent recursion
            this.widthCorrected = true;

            var buttonWidth = this.ownerButton.getWidth(),
                menuWidth = this.getWidth();

            var width = Math.max(buttonWidth, menuWidth);
            this.setWidth(width);
        }
    },

    beforeRender: function () {
        this.callSuper(arguments);
        this.layout.align = 'stretch';
    }

});