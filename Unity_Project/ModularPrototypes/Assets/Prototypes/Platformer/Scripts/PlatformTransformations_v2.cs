using ModularPrototypes.Platformer.Data;

using UnityEngine;

using static ModularPrototypes.Platformer.PlatformTransformationSettings;
using static UnityEngine.Rendering.STP;

namespace ModularPrototypes.Platformer
{
    /// <summary>
    /// This class is responsible for integration with UI.
    /// </summary>
    public class PlatformTransformations_v2 : MonoBehaviour
    {
        [Header("------------------------------------------------------------------------------------------------------")]
        [Header("LOCOMOTION")]
        [Header("------------------------------------------------------------------------------------------------------")]
        [Header("Locomotion Attributes")]
        [SerializeField] private PlatformConfig _translationConfig;
        [Header("Debug Information")]
        [SerializeField] private Vector3 _initialPosition;
        [SerializeField] private float _movementElapsedTime = 0f;

        [Header("------------------------------------------------------------------------------------------------------")]
        [Header("ROTATION")]
        [Header("------------------------------------------------------------------------------------------------------")]
        [Header("Rotation Attributes")]
        [SerializeField] private PlatformConfig _rotationConfig;
        [Header("Debug Information")]
        [SerializeField] private Vector3 _initialRotation;
        [SerializeField] private float _rotationElapsedTime = 0f;

        [Header("------------------------------------------------------------------------------------------------------")]
        [Header("SCALE")]
        [Header("------------------------------------------------------------------------------------------------------")]
        [Header("Scale Attributes")]
        [SerializeField] private PlatformConfig _scaleConfig;
        [Header("Debug Information")]
        [SerializeField] private Vector3 _initialScale;
        [SerializeField] private float _scaleElapsedTime = 0f;

        public void Inititialize(PlatformConfig translationConfig, PlatformConfig rotationConfig, PlatformConfig scaleConfig)
        {
            InitMovementAttributes(translationConfig);
            InitRotationAttributes(rotationConfig);
            InitScaleAttributes(scaleConfig);
        }

        private void Update()
        {
            MovePlatform();
            RotationPlatform();
            ScalePlatform();
        }

        public void ApplyConfiguration(TransformDomain domain, PlatformConfig config)
        {
            switch (domain)
            {
                case TransformDomain.TRANSLATION:
                    ApplyTranslationConfiguration(config);
                    break;
                case TransformDomain.ROTATION:
                    ApplyRotationConfiguration(config);
                    break;
                case TransformDomain.SCALING:
                    ApplyScalingConfiguration(config);
                    break;
            }
        }

        #region MOVEMENT
        private void InitMovementAttributes(PlatformConfig config)
        {
            _movementElapsedTime = 0f;
            _initialPosition = transform.position;
            _translationConfig = config;
        }

        private void ApplyTranslationConfiguration(PlatformConfig config)
        {
            if (_translationConfig == null)
            {
                return;
            }

            _translationConfig.SetConfiguration(config);
        }

        private void MovePlatform()
        {
            if (_translationConfig == null)
            {
                return;
            }

            var normalizedElapsedTime = GetNormalizedElapsedTime(ref _movementElapsedTime, _translationConfig.GetPeriod());

            var position = Vector3.zero;

            var locomotionDataX = _translationConfig.GetPlatformData(TransformAxis.X);
            var locomotionDataY = _translationConfig.GetPlatformData(TransformAxis.Y);
            var locomotionDataZ = _translationConfig.GetPlatformData(TransformAxis.Z);

            switch (_translationConfig.GetTransformDimension())
            {
                case TransformDimension.NONE:
                    break;
                case TransformDimension.X:
                    position += Move_X(locomotionDataX.Amplitude, normalizedElapsedTime, locomotionDataX.Modulo);
                    break;
                case TransformDimension.Y:
                    position += Move_Y(locomotionDataY.Amplitude, normalizedElapsedTime, locomotionDataY.Modulo);
                    break;
                case TransformDimension.Z:
                    position += Move_Z(locomotionDataZ.Amplitude, normalizedElapsedTime, locomotionDataZ.Modulo);
                    break;
                case TransformDimension.XY:
                    position += Move_X(locomotionDataX.Amplitude, normalizedElapsedTime, locomotionDataX.Modulo);
                    position += Move_Y(locomotionDataY.Amplitude, normalizedElapsedTime, locomotionDataY.Modulo);
                    break;
                case TransformDimension.XZ:
                    position += Move_X(locomotionDataX.Amplitude, normalizedElapsedTime, locomotionDataX.Modulo);
                    position += Move_Z(locomotionDataZ.Amplitude, normalizedElapsedTime, locomotionDataZ.Modulo);
                    break;
                case TransformDimension.YZ:
                    position += Move_Z(locomotionDataZ.Amplitude, normalizedElapsedTime, locomotionDataZ.Modulo);
                    position += Move_Y(locomotionDataY.Amplitude, normalizedElapsedTime, locomotionDataY.Modulo);
                    break;
                case TransformDimension.ALL_DIMENSIONS:
                    position += Move_X(locomotionDataX.Amplitude, normalizedElapsedTime, locomotionDataX.Modulo);
                    position += Move_Y(locomotionDataY.Amplitude, normalizedElapsedTime, locomotionDataY.Modulo);
                    position += Move_Z(locomotionDataZ.Amplitude, normalizedElapsedTime, locomotionDataZ.Modulo);
                    break;
            }

            transform.position = _initialPosition + position;
        }

        private Vector3 Move_X(float magnitude, float normalizedElapsedTime, bool useModuloFunction)
        {
            var locomotionDataX = _translationConfig.GetPlatformData(TransformAxis.X);

            return TransformWithFunction(locomotionDataX.Function, magnitude, normalizedElapsedTime, Vector3.right, useModuloFunction, locomotionDataX.Negate);
        }

        private Vector3 Move_Y(float magnitude, float normalizedElapsedTime, bool useModuloFunction)
        {
            var locomotionDataY = _translationConfig.GetPlatformData(TransformAxis.Y);

            return TransformWithFunction(locomotionDataY.Function, magnitude, normalizedElapsedTime, Vector3.up, useModuloFunction, locomotionDataY.Negate);
        }

        private Vector3 Move_Z(float magnitude, float normalizedElapsedTime, bool useModuloFunction)
        {
            var locomotionDataZ = _translationConfig.GetPlatformData(TransformAxis.Z);

            return TransformWithFunction(locomotionDataZ.Function, magnitude, normalizedElapsedTime, Vector3.forward, useModuloFunction, locomotionDataZ.Negate);
        }
        #endregion

        #region ROTATION
        private void InitRotationAttributes(PlatformConfig config)
        {
            _rotationElapsedTime = 0f;
            _initialRotation = transform.rotation.eulerAngles;
            _rotationConfig = config;
        }

        private void ApplyRotationConfiguration(PlatformConfig config)
        {
            if (_rotationConfig == null)
            {
                return;
            }

            _rotationConfig.SetConfiguration(config);
        }

        private void RotationPlatform()
        {
            if (_rotationConfig == null)
            {
                return;
            }

            var normalizedElapsedTime = GetNormalizedElapsedTime(ref _rotationElapsedTime, _rotationConfig.GetPeriod());

            var rotation = Vector3.zero;

            var rotationDataX = _rotationConfig.GetPlatformData(TransformAxis.X);
            var rotationDataY = _rotationConfig.GetPlatformData(TransformAxis.Y);
            var rotationDataZ = _rotationConfig.GetPlatformData(TransformAxis.Z);

            switch (_rotationConfig.GetTransformDimension())
            {
                case TransformDimension.NONE:
                    break;
                case TransformDimension.X:
                    rotation += Rotate_X(rotationDataX.Amplitude, normalizedElapsedTime, rotationDataX.Modulo);
                    break;
                case TransformDimension.Y:
                    rotation += Rotate_Y(rotationDataY.Amplitude, normalizedElapsedTime, rotationDataY.Modulo);
                    break;
                case TransformDimension.Z:
                    rotation += Rotate_Z(rotationDataZ.Amplitude, normalizedElapsedTime, rotationDataZ.Modulo);
                    break;
                case TransformDimension.XY:
                    rotation += Rotate_X(rotationDataX.Amplitude, normalizedElapsedTime, rotationDataX.Modulo);
                    rotation += Rotate_Y(rotationDataY.Amplitude, normalizedElapsedTime, rotationDataY.Modulo);
                    break;
                case TransformDimension.XZ:
                    rotation += Rotate_X(rotationDataX.Amplitude, normalizedElapsedTime, rotationDataX.Modulo);
                    rotation += Rotate_Z(rotationDataZ.Amplitude, normalizedElapsedTime, rotationDataZ.Modulo);
                    break;
                case TransformDimension.YZ:
                    rotation += Rotate_Y(rotationDataY.Amplitude, normalizedElapsedTime, rotationDataY.Modulo);
                    rotation += Rotate_Z(rotationDataZ.Amplitude, normalizedElapsedTime, rotationDataZ.Modulo);
                    break;
                case TransformDimension.ALL_DIMENSIONS:
                    rotation += Rotate_X(rotationDataX.Amplitude, normalizedElapsedTime, rotationDataX.Modulo);
                    rotation += Rotate_Y(rotationDataY.Amplitude, normalizedElapsedTime, rotationDataY.Modulo);
                    rotation += Rotate_Z(rotationDataZ.Amplitude, normalizedElapsedTime, rotationDataZ.Modulo);
                    break;
            }

            transform.eulerAngles = _initialRotation + rotation;
        }

        private Vector3 Rotate_X(float magnitude, float normalizedElapsedTime, bool useModuloFunction)
        {
            var rotationDataX = _translationConfig.GetPlatformData(TransformAxis.X);

            return TransformWithFunction(rotationDataX.Function, magnitude, normalizedElapsedTime, Vector3.right, useModuloFunction, rotationDataX.Negate);
        }

        private Vector3 Rotate_Y(float magnitude, float normalizedElapsedTime, bool useModuloFunction)
        {
            var rotationDataY = _translationConfig.GetPlatformData(TransformAxis.Y);

            return TransformWithFunction(rotationDataY.Function, magnitude, normalizedElapsedTime, Vector3.up, useModuloFunction, rotationDataY.Negate);
        }

        private Vector3 Rotate_Z(float magnitude, float normalizedElapsedTime, bool useModuloFunction)
        {
            var rotationDataZ = _translationConfig.GetPlatformData(TransformAxis.Z);

            return TransformWithFunction(rotationDataZ.Function, magnitude, normalizedElapsedTime, Vector3.forward, useModuloFunction, rotationDataZ.Negate);
        }
        #endregion

        #region SCALE
        private void InitScaleAttributes(PlatformConfig config)
        {
            _scaleElapsedTime = 0f;
            _initialScale = transform.localScale;
            _scaleConfig = config;
        }

        private void ApplyScalingConfiguration(PlatformConfig config)
        {
            if (_scaleConfig == null)
            {
                return;
            }

            _scaleConfig.SetConfiguration(config);
        }

        private void ScalePlatform()
        {
            if (_scaleConfig == null)
            {
                return;
            }

            var normalizedElapsedTime = GetNormalizedElapsedTime(ref _scaleElapsedTime, _scaleConfig.GetPeriod());

            var scale = Vector3.zero;

            var scaleDataX = _scaleConfig.GetPlatformData(TransformAxis.X);
            var scaleDataY = _scaleConfig.GetPlatformData(TransformAxis.Y);
            var scaleDataZ = _scaleConfig.GetPlatformData(TransformAxis.Z);

            switch (_scaleConfig.GetTransformDimension())
            {
                case TransformDimension.NONE:
                    break;
                case TransformDimension.X:
                    scale += Scale_X(scaleDataX.Amplitude, normalizedElapsedTime, scaleDataX.Modulo);
                    break;
                case TransformDimension.Y:
                    scale += Scale_Y(scaleDataY.Amplitude, normalizedElapsedTime, scaleDataY.Modulo);
                    break;
                case TransformDimension.Z:
                    scale += Scale_Z(scaleDataZ.Amplitude, normalizedElapsedTime, scaleDataZ.Modulo);
                    break;
                case TransformDimension.XY:
                    scale += Scale_X(scaleDataX.Amplitude, normalizedElapsedTime, scaleDataX.Modulo);
                    scale += Scale_Y(scaleDataY.Amplitude, normalizedElapsedTime, scaleDataY.Modulo);
                    break;
                case TransformDimension.XZ:
                    scale += Scale_X(scaleDataX.Amplitude, normalizedElapsedTime, scaleDataX.Modulo);
                    scale += Scale_Z(scaleDataZ.Amplitude, normalizedElapsedTime, scaleDataZ.Modulo);
                    break;
                case TransformDimension.YZ:
                    scale += Scale_Y(scaleDataY.Amplitude, normalizedElapsedTime, scaleDataY.Modulo);
                    scale += Scale_Z(scaleDataZ.Amplitude, normalizedElapsedTime, scaleDataZ.Modulo);
                    break;
                case TransformDimension.ALL_DIMENSIONS:
                    scale += Scale_X(scaleDataX.Amplitude, normalizedElapsedTime, scaleDataX.Modulo);
                    scale += Scale_Y(scaleDataY.Amplitude, normalizedElapsedTime, scaleDataY.Modulo);
                    scale += Scale_Z(scaleDataZ.Amplitude, normalizedElapsedTime, scaleDataZ.Modulo);
                    break;
            }

            transform.localScale = _initialScale + scale;
        }

        private Vector3 Scale_X(float magnitude, float normalizedElapsedTime, bool useModuloFunction)
        {
            var scaleDataX = _translationConfig.GetPlatformData(TransformAxis.X);
            return TransformWithFunction(scaleDataX.Function, magnitude, normalizedElapsedTime, Vector3.right, useModuloFunction, scaleDataX.Negate);
        }

        private Vector3 Scale_Y(float magnitude, float normalizedElapsedTime, bool useModuloFunction)
        {
            var scaleDataY = _translationConfig.GetPlatformData(TransformAxis.Y);
            return TransformWithFunction(scaleDataY.Function, magnitude, normalizedElapsedTime, Vector3.up, useModuloFunction, scaleDataY.Negate);
        }

        private Vector3 Scale_Z(float magnitude, float normalizedElapsedTime, bool useModuloFunction)
        {
            var scaleDataZ = _translationConfig.GetPlatformData(TransformAxis.Z);
            return TransformWithFunction(scaleDataZ.Function, magnitude, normalizedElapsedTime, Vector3.forward, useModuloFunction, scaleDataZ.Negate);
        }
        #endregion

        #region UTILITY
        private float GetNormalizedElapsedTime(ref float elapsedTime, float durationPerRound)
        {
            elapsedTime += Time.deltaTime;

            return elapsedTime / durationPerRound;
        }

        private Vector3 TransformWithFunction(TransformFunction function, float magnitude, float normalizedElapsedTime, Vector3 unitDirection, bool useModuloFunction, bool negate)
        {
            var functionOfTime = 0f;

            switch (function)
            {
                case TransformFunction.SIN:
                    functionOfTime = Mathf.Sin(normalizedElapsedTime * 2f * Mathf.PI);
                    break;
                case TransformFunction.COS:
                    functionOfTime = Mathf.Cos(normalizedElapsedTime * 2f * Mathf.PI);
                    break;
            }

            var value = (useModuloFunction ? Mathf.Abs(functionOfTime) : functionOfTime) * magnitude * unitDirection;

            return negate ? -1 * value : value;
        }
        #endregion

        #region DEBUG
        private void D(string message)
        {
            DebugUtils.DebugInfo.Print($"<<PlatformTransformations_v2>> {message}");
        }
        #endregion
    }
}
