export function debounce(fn, delay) {
  let timeout = null;
  return function () {
    clearTimeout(timeout);
    let args = arguments;
    let that = this;
    timeout = setTimeout(function () {
      fn.apply(that, args);
    }, delay);
  };
}

export function capitalizeFirstLetter(string) {
  return string.charAt(0).toUpperCase() + string.slice(1);
}
