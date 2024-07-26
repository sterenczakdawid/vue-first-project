export const pagination = (i18n) => {
  return {
    "items-per-page-options": [5, 10, 15, -1],
    "items-per-page-text": i18n.t("itemsPerPage"),
    "items-per-page-all-text": i18n.t("itemsAllText"),
    "page-text": `{0}-{1} ${i18n.t("of")} {2}`,
  };
};
