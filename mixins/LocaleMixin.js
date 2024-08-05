import { headers } from "~/constants/headers";
import { detailsHeaders } from "~/constants/headers";
import { footer } from "~/constants/footer";

export const LocaleMixin = {
  data() {
    return {
      detailsHeaders: detailsHeaders(this.$i18n),
      headers: headers(this.$i18n),
      footer: footer(this.$i18n),
    };
  },
  methods: {
    localeChanged() {
      this.detailsHeaders = detailsHeaders(this.$i18n);
      this.headers = headers(this.$i18n);
      this.footer = footer(this.$i18n, this.pageSize);
    },
    setTVar() {
      this.$i18n.locale = this.$store.getters.getLang;
    },
  },
  watch: {
    "$i18n.locale": "localeChanged",
    "$store.getters.getLang": "setTVar",
  },
  beforeRouteEnter(_, from, next) {
    next((vm) => {
      vm.setTVar();
    });
  },
};
