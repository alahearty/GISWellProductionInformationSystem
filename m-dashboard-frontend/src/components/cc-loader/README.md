### CcLoader component
A component for creating css coded loaders

## Props

**type**  `type: CcLoaderType`
values: `single-loader(default)', 'array-box', 'array-box-reverse', 'array-train', 'linear'`.
Determines the type of loader you get.


**rounded**  `type: boolean`
Determines if loader will have rounded edges or not.


**height**  `type: number`
Determines height of the loader.

## Slots
There are two slots provided `top` and `bottom`. They can be used to add content to respective positions of the component.

Usage:
```html
<cc-loader>
	<template #top>
		<!-- content -->
	</template>
	<template #bottom>
		<!-- content -->
	</template>
</cc-loader>

```
