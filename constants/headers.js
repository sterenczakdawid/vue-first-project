export const headers = (i18n) => [
  {
    text: "ID",
    align: "start",
    sortable: false,
    value: "id",
  },
  { text: i18n.t("headers.name"), value: "name" },
  { text: i18n.t("headers.created"), value: "created" },
  { text: i18n.t("headers.edited"), value: "edited" },
  { text: i18n.t("headers.actions"), value: "actions", sortable: false },
];

export const detailsHeaders = (i18n) => [
  {
    text: "ID",
    align: "start",
    sortable: false,
    value: "id",
  },
  { text: i18n.t("headers.name"), value: "name" },
  { text: i18n.t("headers.created"), value: "created" },
  { text: i18n.t("headers.edited"), value: "edited" },
];
